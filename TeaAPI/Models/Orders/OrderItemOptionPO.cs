namespace TeaAPI.Models.Orders
{
    public class OrderItemOptionPO
    {
        public int OrderItemId { get; set; }
        public int VariantTypeId { get; set; }
        public string VariantType { get; set; }
        public int VariantValueId { get; set; }
        public string VariantValue { get; set; }
        public decimal ExtraValue { get; set; }
    }
}
