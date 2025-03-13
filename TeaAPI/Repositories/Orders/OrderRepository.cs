using Dapper;
using System.Data;
using TeaAPI.Models.Orders;
using TeaAPI.Repositories.Orders.Interfaces;

namespace TeaAPI.Repositories.Orders
{
    public class OrderRepository : DapperBaseRepository, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CreateAsync(OrderPO order)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Phone", order.Phone);
            parameters.Add("@Title", order.Title);
            parameters.Add("@Address", order.Address);
            parameters.Add("@Remark", order.Remark);
            parameters.Add("@OrderStatus", (int)order.OrderStatus);
            parameters.Add("@OrderDate", order.OrderDate);
            parameters.Add("@TotalAmount", order.TotalAmount);
            parameters.Add("@CreateUser", order.CreateUser);
            parameters.Add("@NewOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("CreateOrder", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@NewOrderId");
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("DeleteOrder", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OrderPO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<OrderPO>("GetAllOrders", commandType: CommandType.StoredProcedure);
        }

        public async Task<OrderPO> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<OrderPO>(
                "GetOrderById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ModifyAsync(OrderPO order)
        {
            using var connection = CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", order.Id);
            parameters.Add("@Phone", order.Phone);
            parameters.Add("@Title", order.Title);
            parameters.Add("@Address", order.Address);
            parameters.Add("@OrderStatus", (int)order.OrderStatus);
            parameters.Add("@OrderDate", order.OrderDate);
            parameters.Add("@TotalAmount", order.TotalAmount);
            parameters.Add("@ModifyUser", order.ModifyUser);

            await connection.ExecuteAsync("ModifyOrder", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
