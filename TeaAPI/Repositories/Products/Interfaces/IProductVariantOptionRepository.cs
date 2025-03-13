using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IProductVariantOptionRepository
    {
        Task<IEnumerable<ProductVariantOptionPO>> GetByProductIdAsync(int productId);
        Task AssignVariantsToProductAsync(int productId, IEnumerable<int> variantValueIds);
        Task DeleteByProductIdAsync(int productId);
    }
}
