using AutoMapper;
using TeaAPI.Dtos.Products;
using TeaAPI.Repositories.Products.Interfaces;
using TeaAPI.Services.Products.Interfaces;

namespace TeaAPI.Services.Products
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;

        public ProductCategoryService(
            IProductCategoryRepository productCategoryRepository, 
            IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductCategoryDTO>> GetAllAsync()
        {
            var categories = await _productCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryDTO>>(categories);
        }

        public async Task<ProductCategoryDTO> GetByIdAsync(int id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);
            return _mapper.Map<ProductCategoryDTO>(category);
        }
    }
}
