using TeaAPI.Models.Accounts;

namespace TeaAPI.Repositories.Accounts.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission> GetByIdAsync(int id);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<IEnumerable<Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds); 
    }
}
