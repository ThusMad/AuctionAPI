using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Media, string>().ConstructUsing(md => md.Url);
            CreateMap<string, Media>().ConstructUsing(x => new Media(x));
        }
    }
}
