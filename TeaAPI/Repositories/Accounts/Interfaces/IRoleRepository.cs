using TeaAPI.Models.Accounts;

namespace TeaAPI.Repositories.Accounts.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
