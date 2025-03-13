using TeaAPI.Dtos.Products;

namespace TeaAPI.Services.Products.Interfaces
{
    public interface IVariantService
    {
        Task<IEnumerable<VariantTypeDTO>> GetVariantTypesWithValueByProductIdAsync(int productId);
        Task<IEnumerable<VariantTypeDTO>> GetAllVariantTypesWithValueAsync();
        Task<IEnumerable<VariantTypeDTO>> GetAllVariantTypesAsync();
        Task<IEnumerable<VariantValueDTO>> GetVariantValuesByTypeIdAsync(int variantTypeId);
        Task AssignVariantsToProductAsync(int productId, IEnumerable<int> variantValueIds);
        Task DeleteByProductIdAsync(int productId);
    }
}
