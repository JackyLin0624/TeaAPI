namespace TeaAPI.Models.Products
{
    public class ProductSizePO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
