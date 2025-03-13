using TeaAPI.Models.Orders;

namespace TeaAPI.Repositories.Orders.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<int> InsertAsync(OrderItemPO item);
        Task BulkInsertAsync(int orderId, IEnumerable<OrderItemPO> orderItems);
        Task<IEnumerable<OrderItemPO>> GetByOrderIdAsync(int orderId);
        Task DeleteByOrderIdAsync(int orderId);
        Task DeleteByIdAsync(int id);
        Task<OrderItemPO> GetByIdAsync(int id);
    }
}
