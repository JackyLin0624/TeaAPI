using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaAPI.Models.Responses;
using TeaAPI.Services.Products.Interfaces;

namespace TeaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Policy = "CanManageProducts")]
    public class VariantController : ControllerBase
    {
        private readonly IVariantService _variantService;

        public VariantController(
            IVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet("GetAllVariantTypes")]
        public async Task<IActionResult> GetAllVariantTypesWithValues()
        {
            try
            {
                var variants = await _variantService.GetAllVariantTypesWithValueAsync();
                return Ok(variants);
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
