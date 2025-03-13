using TeaAPI.Models.Orders.Enums;

namespace TeaAPI.Models.Orders
{
    public class OrderPO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }

        public DateTime CreateAt { get; set; }
        public string CreateUser { get; set; }

        public DateTime ModifyAt { get; set; }
        public string ModifyUser { get; set; }

        public IEnumerable<OrderItemPO> Items { get; set; }
        public string? Remark { get; set; }
    }
}
