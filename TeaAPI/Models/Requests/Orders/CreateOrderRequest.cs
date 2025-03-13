namespace TeaAPI.Models.Requests.Orders
{
    public class CreateOrderRequest
    {
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int SelectedSize { get; set; }
        public string Remark { get; set; }
        public List<ItemOptionRequest> SelectedValues { get; set; }   
    }
    public class ItemOptionRequest
    {
        public int TypeId { get; set; }
        public int ValueId { get; set; }
    }
}
