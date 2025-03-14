using Microsoft.AspNetCore.Mvc;
using TeaAPI.Models.Requests.Auths;
using TeaAPI.Services.Account.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
namespace TeaAPI.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthManagementController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthManagementController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [EnableRateLimiting("LoginRateLimit")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Account, request.Password);
            if (response == null)
                return BadRequest("Invalid username or password");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refresh_token", response.RefreshToken, cookieOptions);
            return Ok(response);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("no refresh token found");
            }
            var response = await _authService.RefreshTokenAsync(refreshToken);
            if (response == null)
                return BadRequest("invalid refresh token");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refresh_token", response.RefreshToken, cookieOptions);
            return Ok(response);
        }
    }
}
