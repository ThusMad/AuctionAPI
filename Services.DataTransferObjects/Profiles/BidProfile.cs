using System.Collections.Generic;
using AutoMapper;
using Services.DataTransferObjects.Objects;
using EPAM_DataAccessLayer.Entities;

namespace Services.DataTransferObjects.Profiles
{
    public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<BidDTO, Bid>();
            CreateMap<Bid, BidDTO>();
        }
}
}
