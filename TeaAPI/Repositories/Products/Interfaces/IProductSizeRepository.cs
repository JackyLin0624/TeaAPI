using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<IEnumerable<ProductSizePO>> GetAllAsync();
        Task<IEnumerable<ProductSizePO>> GetByProductIdAsync(int productId);
        Task UpdateSizesAsync(int productId, IEnumerable<ProductSizePO> sizes);
        Task DeleteByProductIdAsync(int productId);
    }
}
