using TeaAPI.Models.Orders;

namespace TeaAPI.Dtos.Orders
{
    public class OrderItemDTO
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
        public IEnumerable<OrderItemOptionDTO> Options { get; set; }
    }
}
