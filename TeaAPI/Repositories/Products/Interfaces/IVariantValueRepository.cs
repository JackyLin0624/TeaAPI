using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IVariantValueRepository
    {
        Task<IEnumerable<VariantValuePO>> GetAllAsync();
        Task<IEnumerable<VariantValuePO>> GetByTypeIdAsync(int variantTypeId);
        Task<IEnumerable<VariantValuePO>> GetByIdsAsync(IEnumerable<int> variantValueIds);
    }

}
