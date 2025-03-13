using Dapper;
using System.Data;
using TeaAPI.Models.Orders;
using TeaAPI.Repositories.Orders.Interfaces;

namespace TeaAPI.Repositories.Orders
{
    public class OrderItemRepository : DapperBaseRepository, IOrderItemRepository
    {
        public OrderItemRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task BulkInsertAsync(int orderId, IEnumerable<OrderItemPO> orderItems)
        {
            using var connection = CreateConnection();

            var dataTable = new DataTable();
            dataTable.Columns.Add("OrderId", typeof(int));
            dataTable.Columns.Add("ProductName", typeof(string));
            dataTable.Columns.Add("ProductSizeId", typeof(int));
            dataTable.Columns.Add("ProductSizeName", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("Remark", typeof(string));
            dataTable.Columns.Add("CreateUser", typeof(string));

            foreach (var item in orderItems)
            {
                dataTable.Rows.Add(orderId, item.ProductName, item.ProductSizeId, item.ProductSizeName,
                                   item.Price, item.Remark, item.CreateUser);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@OrderItemTable", dataTable.AsTableValuedParameter("OrderItemTableType"));

            await connection.ExecuteAsync("InsertOrderItems", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteByIdAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteOrderItemById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteByOrderIdAsync(int orderId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteOrderItemsByOrderId",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<OrderItemPO> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<OrderItemPO>(
                "GetOrderItemById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<OrderItemPO>> GetByOrderIdAsync(int orderId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<OrderItemPO>(
                "GetOrderItemsByOrderId",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> InsertAsync(OrderItemPO item)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", item.OrderId);
            parameters.Add("@ProductId", item.ProductId);
            parameters.Add("@ProductName", item.ProductName);
            parameters.Add("@ProductSizeId", item.ProductSizeId);
            parameters.Add("@ProductSizeName", item.ProductSizeName);
            parameters.Add("@Count", item.Count);
            parameters.Add("@Price", item.Price);
            parameters.Add("@Remark", item.Remark);
            parameters.Add("@CreateUser", item.CreateUser);
            parameters.Add("@NewOrderItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("InsertOrderItem", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewOrderItemId");
        }
    }
}
