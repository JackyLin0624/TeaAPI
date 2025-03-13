using TeaAPI.Models.Accounts;

namespace TeaAPI.Repositories.Accounts.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByAccountAsync(string account);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SoftDeleteAsync(int id, string deleteUser);
    }
}
