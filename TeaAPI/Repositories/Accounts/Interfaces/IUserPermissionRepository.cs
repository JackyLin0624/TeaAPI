using TeaAPI.Models.Accounts;

namespace TeaAPI.Repositories.Accounts.Interfaces
{
    public interface IUserPermissionRepository
    {
        Task<bool> UserHasPermissionAsync(int userId, int permissionId);
        Task AssignPermissionToUserAsync(int userId, int permissionId);
        Task RemovePermissionFromUserAsync(int userId, int permissionId);
    }
}
