using AutoMapper;
using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Domain.Entities;

namespace CRN_Technical_Assessment.Application.Mapping
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            // Product Mappings
            CreateMap<Products, ProductDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateProductDto, Products>();

            CreateMap<UpdateProductDto, Products>();


            // Category Mappings
            CreateMap<Categories, CategoryDto>();

            CreateMap<CreateCategoryDto, Categories>();

            CreateMap<UpdateCategoryDto, Categories>();


            // User Mappings
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src =>
                        src.Profile != null
                            ? src.Profile.FirstName
                            : string.Empty))
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src =>
                        src.Profile != null
                            ? src.Profile.LastName
                            : string.Empty))
                .ForMember(
                    dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src =>
                        src.Profile != null
                            ? src.Profile.PhoneNumber
                            : string.Empty))
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src =>
                        src.Profile != null
                            ? src.Profile.Address
                            : string.Empty));

            CreateMap<RegisterUserDto, User>()
                .ForMember(
                    dest => dest.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Profile,
                    opt => opt.Ignore());

            CreateMap<RegisterUserDto, CRN_Technical_Assessment.Domain.Entities.Profile>()
                .ForMember(
                    dest => dest.User,
                    opt => opt.Ignore());
        }
    }
}