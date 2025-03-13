using TeaAPI.Dtos.Account;

namespace TeaAPI.Services.Account.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleId(int roleId);
    }
}
