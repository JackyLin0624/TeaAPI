namespace TeaAPI.Dtos.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ProductCategoryDTO Category { get; set; }
        public IEnumerable<ProductSizeDTO> ProductSizes { get; set; }
        public IEnumerable<VariantTypeDTO> Options { get; set; }
    }
}
