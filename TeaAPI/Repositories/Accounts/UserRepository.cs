using Dapper;
using System.Data;
using TeaAPI.Models.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;

namespace TeaAPI.Repositories.Accounts
{
    public class UserRepository : DapperBaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<User>(
                "GetAllUsers",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(User user)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "AddUser",
                new
                {
                    user.Username,
                    user.Account,
                    user.PasswordHash,
                    user.RoleId,
                    CreateAt = DateTime.Now,
                    user.CreateUser
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "UpdateUser",
                new
                {
                    user.Id,
                    user.Username,
                    user.Account,
                    user.RoleId,
                    ModifyAt = DateTime.Now,
                    user.ModifyUser
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SoftDeleteAsync(int id, string deleteUser)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SoftDeleteUser",
                new
                {
                    Id = id,
                    DeleteAt = DateTime.Now,
                    DeleteUser = deleteUser
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User> GetByAccountAsync(string account)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByAccount",
                new { Account = account },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
