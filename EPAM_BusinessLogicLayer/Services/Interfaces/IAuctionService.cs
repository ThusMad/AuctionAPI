using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Payloads;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IAuctionService
    {
        AuctionDTO? CreateAuction(AuctionDTO auctionDto, Guid userId, string userRole);
        void DeleteAuction(Guid id);
        void PlaceBid(BidDTO bid);
        void SubscribeToAuctionBidUpdatesAsync(Guid auctionId, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs);
        IEnumerable<AuctionDTO> GetAll();
        LatestPricePayload CurrentPrice(Guid id);
        AuctionDTO? GetAuction(Guid id);
        ICollection<AuctionDTO> Find(Func<bool, AuctionDTO> predicate);
        ICollection<AuctionDTO> GetOngoing();
        ICollection<AuctionDTO> GetUpcoming();
        ICollection<AuctionDTO> GetRandom();
    }
}