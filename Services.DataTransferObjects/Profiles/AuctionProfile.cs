﻿using System;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using Services.DataTransferObjects.Objects;

namespace Services.DataTransferObjects.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<Auction, AuctionDTO>()
                .ForMember(d => d.Images, o => o.MapFrom(s => s.Images))
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.Categories))
                .ForMember(d => d.Creator, o => o.MapFrom(s => s.Creator));

            CreateMap<AuctionDTO, Auction>()
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .ForMember(d => d.Bids, o => o.Ignore())
                .ForMember(x => x.Categories, opt => opt.MapFrom(a => a.Categories))
                .AfterMap(ModifyCategories);
        }

        private static void ModifyCategories(AuctionDTO auctionDto, Auction auction)
        {
            auction.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (auction.Categories == null) 
                return;

            foreach (var category in auction.Categories)
            {
                category.Auction = auction;
            }
        }

    }
}
