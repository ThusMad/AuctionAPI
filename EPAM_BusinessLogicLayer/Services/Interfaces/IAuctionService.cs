using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.BusinessModels.SocketSlot.Interfaces;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Payloads;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IAuctionService : ISlotProvider
    {
        Task<AuctionDTO> CreateAuction(AuctionDTO auctionDto, Guid userId, string userRole);
        void DeleteAuction(Guid id);
        void PlaceBid(BidDTO bid);
        IEnumerable<AuctionDTO> GetAll(int? limit, int? offset);
        AuctionDTO GetById(Guid id);
        LatestPricePayload CurrentPrice(Guid id);
        IEnumerable<AuctionCategoryDto> GetCategories(int? limit, int? offset);
        Task AddCategoriesAsync(IEnumerable<AuctionCategoryDto> categories);
    }
}