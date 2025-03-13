using TeaAPI.Models.Orders;

namespace TeaAPI.Repositories.Orders.Interfaces
{
    public interface IOrderItemOptionRepository
    {
        Task<IEnumerable<OrderItemOptionPO>> GetByOrderItemIdAsync(int orderItemId);
        Task BulkInsertAsync(int orderItemId, IEnumerable<OrderItemOptionPO> orderOptions);
        Task DeleteByOrderItemIdAsync(int orderItemId);
    }
}
