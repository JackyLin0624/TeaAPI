using TeaAPI.Dtos.Orders;
using TeaAPI.Models.Orders;
using TeaAPI.Repositories.Orders.Interfaces;
using TeaAPI.Services.Orders.Interfaces;

namespace TeaAPI.Services.Orders
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderItemOptionRepository _orderItemOptionRepository;

        public OrderItemService(
            IOrderItemRepository orderItemRepository, 
            IOrderItemOptionRepository orderItemOptionRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderItemOptionRepository = orderItemOptionRepository;
        }

        public async Task CreateAsync(int orderId, OrderItemDTO item, string user)
        {
            var newItem = new OrderItemPO
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductSizeId = item.ProductSizeId,
                ProductSizeName = item.ProductSizeName,
                Count = item.Count,
                Price = item.Price,
                Remark = item.Remark,
                CreateAt = DateTime.UtcNow,
                CreateUser = user 
            };
            int itemId = await _orderItemRepository.InsertAsync(newItem);

            if (itemId <= 0)
            {
                throw new Exception("create order fail");
            }
            if (item.Options != null && item.Options.Any())
            {
                var optionPOs = item.Options.Select(x => new OrderItemOptionPO()
                {
                    ExtraValue = x.ExtraValue,
                    OrderItemId = itemId,
                    VariantType = x.VariantType,
                    VariantTypeId = x.VariantTypeId,
                    VariantValue = x.VariantValue,
                    VariantValueId = x.VariantValueId,
                });
                await _orderItemOptionRepository.BulkInsertAsync(itemId, optionPOs);
            }
               
        }

        public async Task DeleteAsync(int orderId)
        {
            var items = await _orderItemRepository.GetByOrderIdAsync(orderId);
            foreach (var item in items) 
            {
                await _orderItemOptionRepository.DeleteByOrderItemIdAsync(item.Id);
                await _orderItemRepository.DeleteByIdAsync(item.Id);
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _orderItemOptionRepository.DeleteByOrderItemIdAsync(id);
            await _orderItemRepository.DeleteByIdAsync(id);
        }

        public async Task<OrderItemDTO> GetByIdAsync(int id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            var options = await _orderItemOptionRepository.GetByOrderItemIdAsync(id);
            return new OrderItemDTO()
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductSizeId = item.ProductSizeId,
                ProductSizeName = item.ProductSizeName,
                Count = item.Count,
                Price = item.Price,
                Remark = item.Remark,
                Options = options.Select(option => new OrderItemOptionDTO
                {
                   OrderItemId = id,
                   VariantType = option.VariantType,
                   VariantTypeId = option.VariantTypeId,
                   VariantValue = option.VariantValue,
                   VariantValueId = option.VariantValueId,
                   ExtraValue = option.ExtraValue
               }).ToList()
            };
        }

        public async Task<IEnumerable<OrderItemDTO>> GetByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(orderId);
            if (!orderItems.Any())
            {
                return Enumerable.Empty<OrderItemDTO>();
            }
            var orderItemDTOs = orderItems.Select(item => new OrderItemDTO
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductSizeId = item.ProductSizeId,
                ProductSizeName = item.ProductSizeName,
                Count = item.Count,
                Price = item.Price,
                Remark = item.Remark,
                Options = new List<OrderItemOptionDTO>()
            }).ToList();

            foreach (var orderItem in orderItemDTOs)
            {
                var options = await _orderItemOptionRepository.GetByOrderItemIdAsync(orderItem.Id);
                orderItem.Options = options.Select(option => new OrderItemOptionDTO
                {
                    OrderItemId = orderItem.Id,
                    VariantType = option.VariantType,
                    VariantTypeId = option.VariantTypeId,
                    VariantValue = option.VariantValue,
                    VariantValueId = option.VariantValueId,
                    ExtraValue = option.ExtraValue
                }).ToList();
            }
            return orderItemDTOs;
        }

        public async Task UpdateAsync(int id, OrderItemDTO item)
        {
            var existItem = await GetByIdAsync(id);
            if(existItem == null) 
            {
                throw new Exception("order item not exist");
            }
            existItem.Count = item.Count;
            existItem.Price = item.Price;   
            existItem.Remark = item.Remark;
            existItem.ProductId = item.ProductId;
            existItem.ProductName = item.ProductName;
            existItem.ProductSizeId = item.ProductSizeId;
            existItem.ProductSizeName = item.ProductSizeName;
            await _orderItemOptionRepository.DeleteByOrderItemIdAsync(id);
            var optionPOs = item.Options.Select(x => new OrderItemOptionPO()
            {
                ExtraValue = x.ExtraValue,
                OrderItemId = id,
                VariantType = x.VariantType,
                VariantTypeId = x.VariantTypeId,
                VariantValue = x.VariantValue,
                VariantValueId = x.VariantValueId,
            });
            await _orderItemOptionRepository.BulkInsertAsync(id, optionPOs);
        }
    }
}
