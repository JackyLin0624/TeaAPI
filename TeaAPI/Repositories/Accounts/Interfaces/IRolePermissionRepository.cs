using TeaAPI.Models.Accounts;

namespace TeaAPI.Repositories.Accounts.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<RolePermission>> GetByRoleIdAsync(int roleId);
    }
}
