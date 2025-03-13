using TeaAPI.Dtos.Products;

namespace TeaAPI.Services.Products.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryDTO>> GetAllAsync();
        Task<ProductCategoryDTO> GetByIdAsync(int id);
    }
}
