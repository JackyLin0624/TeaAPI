namespace TeaAPI.Models.Products
{
    public class VariantValuePO
    {
        public int Id { get; set; }
        public int VariantTypeId { get; set; }
        public string Value { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
