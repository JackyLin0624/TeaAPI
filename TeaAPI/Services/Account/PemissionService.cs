using TeaAPI.Dtos.Account;
using TeaAPI.Repositories.Accounts.Interfaces;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Services.Account
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public PermissionService(
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDTO
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name
            });
        }

        public async Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleId(int roleId)
        {
            var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(roleId);

            if (!rolePermissions.Any()) return Enumerable.Empty<PermissionDTO>();

            var permissionIds = rolePermissions.Select(rp => rp.PermissionId);
            var permissions = await _permissionRepository.GetPermissionsByIdsAsync(permissionIds);

            return permissions.Select(p => new PermissionDTO
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name
            });

        }
    }

}
