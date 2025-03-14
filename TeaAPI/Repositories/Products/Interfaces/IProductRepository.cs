using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductPO>> GetAllAsync();
        Task<IEnumerable<ProductPO>> GetByCategoryIdAsync(int categoryId);
        Task<ProductPO> GetByIdAsync(int id, bool includeDeleted = false);
        Task<int> CreateAsync(ProductPO product);
        Task ModifyAsync(ProductPO product);
        Task DeleteAsync(int id);
    }
}
