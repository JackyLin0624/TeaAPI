using TeaAPI.Dtos.Account;
using TeaAPI.Models.Responses;

namespace TeaAPI.Services.Account.Interfaces
{
    public interface IUserService
    {
        Task<ResponseBase> CreateUserAsync(string name, string account, string password, int roleId, string createUser);
        Task DeleteUserAsync(int id, string user);
        Task<ResponseBase> ModifyUserAsync(int id, string name, int roleId, string modifyUser);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserWithPasswordDTO> GetByAccountAsync(string account);
    }
}
