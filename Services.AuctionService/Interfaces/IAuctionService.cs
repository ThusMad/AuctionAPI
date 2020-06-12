using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.AuctionService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuctionService 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="auctionDto"></param>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        Task<AuctionDTO> InsertAuctionAsync(AuctionDTO auctionDto, Guid userId, string userRole);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        Task<AuctionDTO> UpdateAuctionAsync(AuctionDTO newItem);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAuctionAsync(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<IEnumerable<AuctionDTO>> GetAllAsync(string? filters, int? limit, int? offset);
        /// <summary>
        /// Returns instance of auction with following id transformed to <see cref="AuctionDTO"/>
        /// </summary>
        /// <param name="id">id of searched auction</param>
        /// <returns><see cref="AuctionDTO"/> instance</returns>
        Task<AuctionDTO> GetByIdAsync(Guid id);

        Task AttachMediaAsync(Guid auctionId, Guid userId, string[] mediaUrls);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auctionId"></param>
        /// <param name="userId"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        Task<BidDTO> InsertBidAsync(Guid auctionId, Guid userId, decimal price);
    }
}