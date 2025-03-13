using TeaAPI.Dtos.Orders;

namespace TeaAPI.Services.Orders.Interfaces
{
    public interface IOrderItemService
    {
        Task CreateAsync(int orderId, OrderItemDTO item, string user);
        Task DeleteAsync(int orderId);
        Task<IEnumerable<OrderItemDTO>> GetByOrderIdAsync(int orderId);
        Task UpdateAsync(int id, OrderItemDTO item);
        Task DeleteByIdAsync(int id);
        Task<OrderItemDTO> GetByIdAsync(int id);
    }
}
