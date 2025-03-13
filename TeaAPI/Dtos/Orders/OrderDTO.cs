using TeaAPI.Models.Orders.Enums;
using TeaAPI.Models.Orders;

namespace TeaAPI.Dtos.Orders
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
        public string? Remark { get; set; }
    }
}
