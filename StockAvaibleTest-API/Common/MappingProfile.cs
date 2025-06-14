using AutoMapper;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

            // Product mappings
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, 
                    opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            // Box mappings
            CreateMap<Box, BoxDTO>();
            CreateMap<Box, BoxDetailDTO>();
            CreateMap<CreateBoxDTO, Box>();
            CreateMap<UpdateBoxDTO, Box>();

            // Transaction mappings
            CreateMap<BoxProductTransaction, TransactionDTO>()
                .ForMember(dest => dest.BoxCode, 
                    opt => opt.MapFrom(src => src.Box.Code))
                .ForMember(dest => dest.ProductCode, 
                    opt => opt.MapFrom(src => src.Product.Code))
                .ForMember(dest => dest.ProductDescription, 
                    opt => opt.MapFrom(src => src.Product.Description));
            CreateMap<CreateTransactionDTO, BoxProductTransaction>();
        }
    }
}
