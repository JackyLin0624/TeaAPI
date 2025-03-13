using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategoryPO>> GetAllAsync();
        Task<ProductCategoryPO> GetByIdAsync(int id);
    }
}
