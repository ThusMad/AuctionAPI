using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
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
