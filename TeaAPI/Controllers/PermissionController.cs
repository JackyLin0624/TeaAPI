using Microsoft.AspNetCore.Mvc;
using TeaAPI.Models.Responses;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("GetAllPermissions")]
        public async Task<IActionResult> GetAllPermissionsAsync()
        {
            try
            {
                var res = await _permissionService.GetAllPermissionsAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBase()
                {
                    ResultCode = -999,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
