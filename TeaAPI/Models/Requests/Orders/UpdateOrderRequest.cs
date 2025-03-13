namespace TeaAPI.Models.Requests.Orders
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public int OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }
}
