using Dapper;
using System.Data;
using TeaAPI.Repositories.Accounts.Interfaces;

namespace TeaAPI.Repositories.Accounts
{
    public class UserPermissionRepository : DapperBaseRepository, IUserPermissionRepository
    {
        public UserPermissionRepository(IConfiguration configuration) : base(configuration) { }

        public async Task AssignPermissionToUserAsync(int userId, int permissionId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "AssignPermissionToUser",
                new { UserId = userId, PermissionId = permissionId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemovePermissionFromUserAsync(int userId, int permissionId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "RemovePermissionFromUser",
                new { UserId = userId, PermissionId = permissionId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UserHasPermissionAsync(int userId, int permissionId)
        {
            using var connection = CreateConnection();
            var existing = await connection.ExecuteScalarAsync<int>(
                "UserHasPermission",
                new { UserId = userId, PermissionId = permissionId },
                commandType: CommandType.StoredProcedure
            );
            return existing > 0;
        }
    }

}
