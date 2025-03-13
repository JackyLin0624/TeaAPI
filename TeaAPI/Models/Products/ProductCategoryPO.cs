namespace TeaAPI.Models.Products
{
    public class ProductCategoryPO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreateUser { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyUser { get; set; }
    }
}
