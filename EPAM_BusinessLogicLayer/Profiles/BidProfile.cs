using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObjects;
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
