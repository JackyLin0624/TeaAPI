using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeaAPI.Models.Requests.Products;
using TeaAPI.Models.Responses;
using TeaAPI.Services.Products.Interfaces;

namespace TeaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Policy = "CanManageProducts")]
        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> Delete(DeleteProductRequest request)
        {
            try
            {
                return Ok(await _productService.DeleteAsync(request.Id));

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

        [Authorize(Policy = "CanManageProductsOrOrder")]
        [HttpPost("GetProductById")]
        public async Task<IActionResult> GetByIdAsync(GetProductRequest request)
        {
            try
            {
                return Ok(await _productService.GetByIdAsync(request.Id, true));

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

        [Authorize(Policy = "CanManageProducts")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProductAsync(CreateProductRequest request)
        {
            try
            {
                string user = GetUserId();
                var res = await _productService.CreateAsync(request, user);
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

        [Authorize(Policy = "CanManageProductsOrOrder")]
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _productService.GetAllAsync());

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

        [Authorize(Policy = "CanManageProducts")]
        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductRequest request)
        {
            try
            {
                string user = GetUserId();
                var res = await _productService.UpdateAsync(request, user);
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

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
        }
    }
}
