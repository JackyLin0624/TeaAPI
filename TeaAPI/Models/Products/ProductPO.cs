namespace TeaAPI.Models.Products
{
    public class ProductPO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateUser { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyUser { get; set; }

    }
}
