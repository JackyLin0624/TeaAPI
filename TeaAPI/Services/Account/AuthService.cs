using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TeaAPI.Models.Auths;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Services.Account
{
    public class AuthService : IAuthService
    {
     
        private readonly JWTConfig _jwtConfig;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IRefreshTokenCacheService _refreshTokenCacheService;

        public AuthService(
            IOptions<JWTConfig> jwtConfig,
            IUserService userService,
            IPasswordService passwordService,
            IRefreshTokenCacheService refreshTokenCacheService)
        {
            _jwtConfig = jwtConfig.Value;
            _userService = userService;
            _passwordService = passwordService;
            _refreshTokenCacheService = refreshTokenCacheService;
        }

        public async Task<AuthResponse> LoginAsync(string account, string password)
        {
            var user = await _userService.GetByAccountAsync(account);
            if (user == null)
                return null;
            var isMatch = _passwordService.VerifyPassword(password, user.PasswordHash);
            if (!isMatch)
            {
                return null;
            }

            var accessToken = GenerateAccessToken(user.Id, user.Account, user.Role.Name, user.Role.Permissions.Select(x => x.Name));
            var refreshToken = GenerateRefreshToken();

            await _refreshTokenCacheService.SetRefreshTokenAsync(user.Id, refreshToken, TimeSpan.FromDays(7));


            return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private string GenerateAccessToken(int userId, string account, string role, IEnumerable<string> policies)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, account),  
                new Claim(ClaimTypes.Role, role),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var policy in policies)
            {
                claims.Add(new Claim("Permission", policy));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumGenerator = RandomNumberGenerator.Create();
            randomNumGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var userId = await _refreshTokenCacheService.GetUserIdByRefreshTokenAsync(refreshToken);
            if (userId == null)
            {
                return null; 
            }
            await _refreshTokenCacheService.RemoveRefreshTokenAsync(refreshToken);

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return null;
            }
 
            var newAccessToken = GenerateAccessToken(user.Id, user.Account, user.Role.Name, user.Role.Permissions.Select(x => x.Name));
            var newRefreshToken = GenerateRefreshToken();

            await _refreshTokenCacheService.SetRefreshTokenAsync(user.Id, newRefreshToken, TimeSpan.FromDays(7));

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


        public ClaimsPrincipal ValidateExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            if (securityToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }

            return null;
        }
    }
}
