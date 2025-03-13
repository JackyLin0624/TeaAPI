using TeaAPI.Dtos.Account;
using TeaAPI.Models.Accounts;
using TeaAPI.Models.Responses;
using TeaAPI.Repositories.Accounts.Interfaces;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Services.Account
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;

        public UserService(
            IPasswordService passwordService,
            IUserRepository userRepository,
            IRoleService roleService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
            _roleService = roleService;
        }



        public async Task<ResponseBase> CreateUserAsync(string name, string account, string password, int roleId, string createUser)
        {
            var existingUser = await _userRepository.GetByAccountAsync(account);
            if (existingUser != null)
            {
                return new ResponseBase
                {
                    ResultCode = -1,
                    Errors = new List<string> { "account duplicate" }
                };
            }
            var role = await _roleService.GetByIdAsync(roleId);
            if(role == null) 
            {
                return new ResponseBase
                {
                    ResultCode = -2,
                    Errors = new List<string> { $"role id : {roleId} not exist" }
                };
            }
            var hashedPassword = _passwordService.HashPassword(password);
            
            var newUser = new User
            {
                Username = name,
                Account = account,
                PasswordHash = hashedPassword,
                RoleId = roleId,
                IsDeleted = false,
                CreateAt = DateTime.UtcNow,
                CreateUser = createUser
            };

            await _userRepository.AddAsync(newUser);
            return new ResponseBase() { ResultCode = 0 };

        }


        public async Task DeleteUserAsync(int id, string user)
        {
            await _userRepository.SoftDeleteAsync(id, user);
        }

  
        public async Task<ResponseBase> ModifyUserAsync(int id, string name, int roleId, string modifyUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return new ResponseBase() 
                {
                    ResultCode = -1,
                    Errors = new List<string> { "user not exist" }
                };
            }
            var role = await _roleService.GetByIdAsync(roleId);
            if (role == null)
            {
                return new ResponseBase()
                {
                    ResultCode = -2,
                    Errors = new List<string> { $"role id : {roleId} not exist" }
                };    
            }
            existingUser.Username = name;
            existingUser.RoleId = roleId;
            existingUser.ModifyAt = DateTime.UtcNow;
            existingUser.ModifyUser = modifyUser;

            await _userRepository.UpdateAsync(existingUser);
            return new ResponseBase() { ResultCode = 0 };
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = (await _userRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var roles = (await _roleService.GetAllRolesAsync()).ToDictionary(r => r.Id);

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Account = u.Account,
                Role = roles.ContainsKey(u.RoleId) ? roles[u.RoleId] : null
            });
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var role = await _roleService.GetByIdAsync(user.RoleId);
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Account = user.Account,
                Role = role
            };
        }

        public async Task<UserWithPasswordDTO> GetByAccountAsync(string account)
        {
            var user = await _userRepository.GetByAccountAsync(account);
            if (user == null)
            {
                return null;
            }
            var role = await _roleService.GetByIdAsync(user.RoleId);
            return new UserWithPasswordDTO
            {
                Id = user.Id,
                Username = user.Username,
                Account = user.Account,
                Role = role,
                PasswordHash = user.PasswordHash
            };
        }
    }

}
