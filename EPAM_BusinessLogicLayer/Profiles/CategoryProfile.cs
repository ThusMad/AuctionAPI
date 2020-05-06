using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
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
                .ForMember(a => a.Category, opt => opt.MapFrom(a => a));

            CreateMap<AuctionCategoryDto, Category>();
            CreateMap<Category, AuctionCategoryDto>();
        }
    }
}
