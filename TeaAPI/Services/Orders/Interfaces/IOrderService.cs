using TeaAPI.Dtos.Orders;
using TeaAPI.Models.Requests.Orders;
using TeaAPI.Models.Responses;

namespace TeaAPI.Services.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseBase> CreateAsync(CreateOrderRequest order, string user);
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO> GetByIdAsync(int id);
        Task<ResponseBase> UpdateAsync(UpdateOrderRequest order, string user);
        Task<ResponseBase> DeleteAsync(int id);
    }
}
