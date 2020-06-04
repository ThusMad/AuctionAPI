﻿using AutoMapper;
using Services.DataTransferObjects.Objects;
using EPAM_DataAccessLayer.Entities;
using Services.DataTransferObjects.Extensions;

namespace Services.DataTransferObjects.Profiles
{
    class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserPreviewDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dto => dto.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate));

            CreateMap<ApplicationUserPatchModel, ApplicationUser>().MapOnlyIfChanged();
        }
    }
}