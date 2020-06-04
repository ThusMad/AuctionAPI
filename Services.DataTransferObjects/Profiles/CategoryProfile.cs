using AutoMapper;
using Services.DataTransferObjects.Objects;
using EPAM_DataAccessLayer.Entities;

namespace Services.DataTransferObjects.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AuctionCategory, AuctionCategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(a => a.Category.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(a => a.Category.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(a => a.Category.Description));

            CreateMap<AuctionCategoryDto, AuctionCategory>()
                .ForMember(a => a.CategoryId, opt => opt.MapFrom(a => a.Id));

            CreateMap<AuctionCategoryDto, Category>();
            CreateMap<Category, AuctionCategoryDto>();
        }
    }
}
