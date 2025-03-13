namespace TeaAPI.Models.Orders
{
    public class OrderItemPO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductSizeId { get; set; }
        public int Count { get; set; }
        public string ProductSizeName { get; set; }
        public decimal Price { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateUser { get; set; }
        public IEnumerable<OrderItemOptionPO> Options { get; set; }
    }
}
