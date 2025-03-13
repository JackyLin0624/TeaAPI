using Dapper;
using System.Data;
using TeaAPI.Models.Orders;
using TeaAPI.Repositories.Orders.Interfaces;

namespace TeaAPI.Repositories.Orders
{
    public class OrderItemOptionRepository : DapperBaseRepository, IOrderItemOptionRepository
    {
        public OrderItemOptionRepository(
            IConfiguration configuration) : base(configuration)
        {
        }

        public async Task DeleteByOrderItemIdAsync(int orderItemId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteOrderItemOptionsByOrderItemId",
                new { OrderItemId = orderItemId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<OrderItemOptionPO>> GetByOrderItemIdAsync(int orderItemId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<OrderItemOptionPO>(
                "GetOrderItemOptionsByOrderItemId",
                new { OrderItemId = orderItemId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task BulkInsertAsync(int orderItemId, IEnumerable<OrderItemOptionPO> orderOptions)
        {
            if (!orderOptions.Any()) return;

            using var connection = CreateConnection();
            var dataTable = new DataTable();
            dataTable.Columns.Add("OrderItemId", typeof(int));  
            dataTable.Columns.Add("VariantTypeId", typeof(int));
            dataTable.Columns.Add("VariantType", typeof(string));
            dataTable.Columns.Add("VariantValueId", typeof(int));
            dataTable.Columns.Add("VariantValue", typeof(string));
            dataTable.Columns.Add("ExtraValue", typeof(decimal));

            foreach (var option in orderOptions)
            {
                dataTable.Rows.Add(orderItemId, option.VariantTypeId, option.VariantType, option.VariantValueId, option.VariantValue, option.ExtraValue);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@OrderItemId", orderItemId);
            parameters.Add("@OrderItemOptions", dataTable.AsTableValuedParameter("OrderItemOptionsTableType"));

            await connection.ExecuteAsync("InsertOrderItemOptions", parameters, commandType: CommandType.StoredProcedure);
        }

     
    }
}
