using TeaAPI.Dtos.Account;

namespace TeaAPI.Services.Account.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDTO> GetByIdAsync(int id);
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    }
}
