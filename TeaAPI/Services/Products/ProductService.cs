using AutoMapper;
using TeaAPI.Dtos.Products;
using TeaAPI.Models.Products;
using TeaAPI.Models.Requests.Products;
using TeaAPI.Models.Responses;
using TeaAPI.Repositories.Products.Interfaces;
using TeaAPI.Services.Products.Interfaces;

namespace TeaAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IVariantService _variantService;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IProductSizeRepository productSizeRepository,
            IVariantService variantService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _productSizeRepository = productSizeRepository;
            _variantService = variantService;
            _mapper = mapper;
        }

        public async Task<ResponseBase> CreateAsync(CreateProductRequest request, string user)
        {
            var category = await _productCategoryService.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return new ResponseBase()
                {
                    ResultCode = -1,
                    Errors = new List<string>() { "category not exist" }
                };
            }
            var product = new ProductPO()
            {
                ProductName = request.Name,
                CategoryId = request.CategoryId,
                Description = request.Description,
                IsActive = request.IsActive,
                CreateAt = DateTime.Now,
                CreateUser = user
            };

            var productId = await _productRepository.CreateAsync(product);

            if (request.ProductSizes != null && request.ProductSizes.Any())
            {

                var productSizes = request.ProductSizes.Select(x => _mapper.Map<ProductSizePO>(x));
                await _productSizeRepository.UpdateSizesAsync(productId, productSizes);
            }

            if(request.Options != null && request.Options.Any()) 
            {
                await _variantService.AssignVariantsToProductAsync(productId, request.Options);
            }
            return new ResponseBase()
            {
                ResultCode = 0
            };
        }

        public async Task<ResponseBase> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseBase()
                {
                    ResultCode = -1,
                    Errors = new List<string>() { "product not exist" }
                };
            }
            //await _variantService.DeleteByProductIdAsync(id);
            //await _productSizeRepository.DeleteByProductIdAsync(id); => modify to use soft delete
            await _productRepository.DeleteAsync(id);
            return new ResponseBase()
            {
                ResultCode = 0
            };
        }

        public async Task<IEnumerable<ProductDTO>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetActiveProductsByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            var activeProducts = products.Where(p => p.IsActive);
            return _mapper.Map<IEnumerable<ProductDTO>>(activeProducts);
        }


        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (!products.Any())
            {
                return Enumerable.Empty<ProductDTO>();
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            var categoryDict = (await _productCategoryService.GetAllAsync())
                .ToDictionary(c => c.Id);

            var productSizes = await _productSizeRepository.GetAllAsync();
            var productSizeDict = productSizes
                .GroupBy(ps => ps.ProductId)
                .ToDictionary(g => g.Key, g => _mapper.Map<IEnumerable<ProductSizeDTO>>(g.ToList()));

            foreach (var productDto in productDtos)
            {
                if (categoryDict.TryGetValue(productDto.CategoryId, out var categoryDto))
                {
                    productDto.Category = categoryDto;
                }

                if (productSizeDict.TryGetValue(productDto.Id, out var sizes))
                {
                    productDto.ProductSizes = sizes;
                }
                else
                {
                    productDto.ProductSizes = Enumerable.Empty<ProductSizeDTO>();
                }
            }

            return productDtos;
        }

        public async Task<ProductDTO> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var product = await _productRepository.GetByIdAsync(id, includeDeleted);
            if (product == null)
            {
                return null;
            }
            var productDto = _mapper.Map<ProductDTO>(product);
            var categoryDto = await _productCategoryService.GetByIdAsync(product.CategoryId);
            productDto.Category = categoryDto;

            var productSizes = await _productSizeRepository.GetByProductIdAsync(id, includeDeleted);
            productDto.ProductSizes = _mapper.Map<IEnumerable<ProductSizeDTO>>(productSizes);

            productDto.Options = await _variantService.GetVariantTypesWithValueByProductIdAsync(id);
            return productDto;
        }

        public async Task<ResponseBase> UpdateAsync(UpdateProductRequest request, string user)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.Id);
            if (existingProduct == null)
            {
                return new ResponseBase()
                {
                    ResultCode = -1,
                    Errors = new List<string>() { "product not exist" }
                };
            }

            existingProduct.ProductName = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.IsActive = request.IsActive;
            existingProduct.ModifyAt = DateTime.Now;
            existingProduct.ModifyUser = user;
            await _productRepository.ModifyAsync(existingProduct);
            if (request.ProductSizes != null && request.ProductSizes.Any())
            {
                var updateSizes = new List<ProductSizePO>();
                var existSizes = (await _productSizeRepository.GetByProductIdAsync(request.Id)).ToDictionary(x => x.Size);

                foreach (var productSize in request.ProductSizes)
                {
                    if (existSizes.ContainsKey(productSize.Size))
                    {
                        existSizes[productSize.Size].Price = productSize.Price;
                        updateSizes.Add(existSizes[productSize.Size]);
                    }
                    else
                    {
                        updateSizes.Add(_mapper.Map<ProductSizePO>(productSize));
                    }
                }
                
                await _productSizeRepository.UpdateSizesAsync(request.Id, updateSizes);
            }
            if (request.Options != null && request.Options.Any())
            {
                await _variantService.AssignVariantsToProductAsync(request.Id, request.Options);
            }
            return new ResponseBase()
            {
                ResultCode = 0
            };
        }
    }

}
