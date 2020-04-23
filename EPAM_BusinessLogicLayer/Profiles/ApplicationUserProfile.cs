using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dto => dto.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate));
        }
    }
}
