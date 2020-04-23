using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Profiles
{
    public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<BidDTO, Bid>();
            CreateMap<Bid, BidDTO>();
            //CREATE PROCEDURE[dbo].[GetLattestBid]
            //@AuctionID int
            //    AS
            //BEGIN
            //    SET NOCOUNT ON;
            //SELECT TOP(1) Price
            //    FROM Bids
            //    ORDER BY Bids.Time
            //    END

        }
}
}
