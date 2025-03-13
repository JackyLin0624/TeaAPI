using AutoMapper;
using TeaAPI.Dtos.Products;
using TeaAPI.Models.Products;

namespace TeaAPI.AutoMappers
{
    public class TeaMappingProfile : Profile
    {
        public TeaMappingProfile()
        {
            CreateMap<ProductCategoryPO, ProductCategoryDTO>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            CreateMap<ProductPO, ProductDTO>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName));

            CreateMap<IEnumerable<ProductPO>, IEnumerable<ProductDTO>>()
           .ConvertUsing((src, dest, context) =>
               src.Select(p => context.Mapper.Map<ProductDTO>(p)).ToList());

            CreateMap<ProductSizePO, ProductSizeDTO>().ReverseMap();
        }
    }
}
