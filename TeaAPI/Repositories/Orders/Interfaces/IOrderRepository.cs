using TeaAPI.Models.Orders;

namespace TeaAPI.Repositories.Orders.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(OrderPO order);
        Task DeleteAsync(int id);
        Task<IEnumerable<OrderPO>> GetAllAsync();
        Task<OrderPO> GetByIdAsync(int id);
        Task ModifyAsync(OrderPO order);
    }
}
