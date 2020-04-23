using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<Media, string>().ConstructUsing(md => md.Url);
            CreateMap<Auction, AuctionDTO>().ForMember(d => d.Images, o => o.MapFrom(s => s.Images));
            CreateMap<AuctionDTO, Auction>().ForMember(x => x.Images, opt => opt.Ignore());
        }
    }
}
