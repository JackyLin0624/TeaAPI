using TeaAPI.Dtos.Account;
using TeaAPI.Repositories.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Services.Account
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public RoleService(
            IRoleRepository roleRepository, 
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        public async Task<RoleDTO> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return null;

            var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(id);
            var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

            var permissions = await _permissionRepository.GetPermissionsByIdsAsync(permissionIds);

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions.Select(p => new PermissionDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name
                })
            };
        }
    }
}
