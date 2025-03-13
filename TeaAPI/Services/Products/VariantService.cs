using TeaAPI.Dtos.Products;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;
using TeaAPI.Services.Products.Interfaces;

namespace TeaAPI.Services.Products
{
    public class VariantService : IVariantService
    {
        private readonly IVariantTypeRepository _variantTypeRepository;
        private readonly IVariantValueRepository _variantValueRepository;
        private readonly IProductVariantOptionRepository _productVariantOptionRepository;
        public VariantService(
        IVariantTypeRepository variantTypeRepository,
        IVariantValueRepository variantValueRepository,
        IProductVariantOptionRepository productVariantOptionRepository)
        {
            _variantTypeRepository = variantTypeRepository;
            _variantValueRepository = variantValueRepository;
            _productVariantOptionRepository = productVariantOptionRepository;
        }

        public async Task<IEnumerable<VariantTypeDTO>> GetAllVariantTypesAsync()
        {
            var types = await _variantTypeRepository.GetAllAsync();
            return types.Select(t => new VariantTypeDTO { Id = t.Id, TypeName = t.TypeName });
        }

        public async Task<IEnumerable<VariantValueDTO>> GetVariantValuesByTypeIdAsync(int variantTypeId)
        {
            var values = await _variantValueRepository.GetByTypeIdAsync(variantTypeId);
            return values.Select(v => new VariantValueDTO { Id = v.Id, Value = v.Value, ExtraPrice = v.ExtraPrice });
        }

        public async Task AssignVariantsToProductAsync(int productId, IEnumerable<int> variantValueIds)
        {
            await _productVariantOptionRepository.AssignVariantsToProductAsync(productId, variantValueIds);
        }

        public async Task<IEnumerable<VariantTypeDTO>> GetAllVariantTypesWithValueAsync()
        {
            var variantTypes = await _variantTypeRepository.GetAllAsync();
            var variantValues = await _variantValueRepository.GetAllAsync();

            var variantTypeDict = variantTypes.ToDictionary(vt => vt.Id, vt => new VariantTypeDTO
            {
                Id = vt.Id,
                TypeName = vt.TypeName,
                VariantValues = new List<VariantValueDTO>()
            });

            foreach (var value in variantValues)
            {
                if (variantTypeDict.TryGetValue(value.VariantTypeId, out var variantTypeDto))
                {
                    variantTypeDto.VariantValues.Add(new VariantValueDTO
                    {
                        TypeId = variantTypeDto.Id,
                        Id = value.Id,
                        Value = value.Value,
                        ExtraPrice = value.ExtraPrice
                    });
                }
            }

            return variantTypeDict.Values;
        }

        public async Task<IEnumerable<VariantTypeDTO>> GetVariantTypesWithValueByProductIdAsync(int productId)
        {
            var productVariantOptions = await _productVariantOptionRepository.GetByProductIdAsync(productId);
            if (!productVariantOptions.Any())
            {
                return Enumerable.Empty<VariantTypeDTO>();
            }

           //for search
            var variantTypesDict = (await _variantTypeRepository.GetAllAsync())
                .ToDictionary(x => x.Id, x => x.TypeName);
  
            var variantValueIds = productVariantOptions
                .Select(v => v.VariantValueId)
                .Distinct()
                .ToList();

            var variantValues = await _variantValueRepository.GetByIdsAsync(variantValueIds);

            var variantTypeDict = new Dictionary<int, VariantTypeDTO>();

            foreach (var variant in variantValues)
            {
                if (!variantTypesDict.TryGetValue(variant.VariantTypeId, out var typeName))
                    continue;
 
                if (!variantTypeDict.TryGetValue(variant.VariantTypeId, out var variantTypeDto))
                {
                    variantTypeDto = new VariantTypeDTO
                    {
                        Id = variant.VariantTypeId,
                        TypeName = typeName,
                        VariantValues = new List<VariantValueDTO>()
                    };
                    variantTypeDict[variant.VariantTypeId] = variantTypeDto;
                }

                variantTypeDto.VariantValues.Add(new VariantValueDTO
                {
                    TypeId = variantTypeDto.Id,
                    Id = variant.Id,
                    Value = variant.Value,
                    ExtraPrice = variant.ExtraPrice
                });
            }

            return variantTypeDict.Values;
        }

        public async Task DeleteByProductIdAsync(int productId)
        {
            await _productVariantOptionRepository.DeleteByProductIdAsync(productId);
        }
    }

}
