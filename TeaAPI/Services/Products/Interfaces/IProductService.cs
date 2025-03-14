using TeaAPI.Dtos.Products;
using TeaAPI.Models.Requests.Products;
using TeaAPI.Models.Responses;

namespace TeaAPI.Services.Products.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<ProductDTO>> GetActiveProductsAsync();
        Task<ProductDTO> GetByIdAsync(int id, bool includeDeleted = false);
        Task<IEnumerable<ProductDTO>> GetActiveProductsByCategoryIdAsync(int categoryId);
        Task<ResponseBase> CreateAsync(CreateProductRequest request, string user);
        Task<ResponseBase> UpdateAsync(UpdateProductRequest request, string user);
        Task<ResponseBase> DeleteAsync(int id);
    }
}
