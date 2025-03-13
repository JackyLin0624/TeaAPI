namespace TeaAPI.Dtos.Products
{
    public class VariantTypeDTO
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<VariantValueDTO> VariantValues { get; set; }
    }
}
