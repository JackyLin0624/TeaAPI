using TeaAPI.Dtos.Products;
using TeaAPI.Models.Products;

namespace TeaAPI.Repositories.Products.Interfaces
{
    public interface IVariantTypeRepository
    {
        Task<IEnumerable<VariantTypePO>> GetAllAsync();
        Task<VariantTypePO> GetByIdAsync(int id);     
    }
}
