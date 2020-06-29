using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.AuctionService.Extensions;
using Services.AuctionService.Interfaces;
using Services.DataTransferObjects.Objects;
using Services.Infrastructure.Exceptions;
using AccessViolationException = Services.Infrastructure.Exceptions.AccessViolationException;

namespace Services.AuctionService.Service
{
    /// <summary>
    /// This service provides general CRUD operations
    /// </summary>
    public class AuctionService : IAuctionService
    {
        /// <summary>
        /// Mapper for mapping entities to DTO's
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Unit of work that provides CRUD operations to database
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        /// <summary>
        /// Constructor that create <seealso cref="AuctionService"/> instance
        /// </summary>
        /// <param name="mapper">Mapper <seealso cref="IMapper"/></param>
        /// <param name="unitOfWork">Unit of work <seealso cref="IUnitOfWork"/></param>
        public AuctionService(IMapper mapper, IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger(typeof(AuctionService));
            _mapper = mapper;
        }

        /// <summary>
        /// This method inserts <seealso cref="AuctionDTO"/> into database
        /// </summary>
        /// <param name="auctionDto">DTO that contains valid auction information</param>
        /// <param name="userId">Id of user that creates auction</param>
        /// <param name="userRole">Role of user that creates auction</param>
        /// <returns>Result of insertion</returns>
        /// <exception cref="ValidationException">Throws when data provided in <see cref="AuctionDTO"/> not matching</exception>
        public async Task<AuctionDTO> InsertAuctionAsync(AuctionDTO auctionDto, Guid userId, string userRole)
        {
            if (auctionDto.StartTime > auctionDto.EndTime && auctionDto.AuctionType == AuctionType.Normal)
            {
                throw new ValidationException($"{nameof(auctionDto.StartTime)} can't be ahead of the {nameof(auctionDto.EndTime)}", nameof(auctionDto));
            }

            var auction = _mapper.Map<AuctionDTO, Auction>(auctionDto,opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.UserId = userId.ToString();
                }));

            await _unitOfWork.InsertAsync(auction);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<Auction, AuctionDTO>(auction);
        }

        /// <summary>
        /// Return collection of <see cref="AuctionDTO"/> selected from database
        /// </summary>
        /// <param name="filters">filters that used to select specific entities</param>
        /// <param name="limit">maximal number of entities to select</param>
        /// <param name="offset">offset entities to select from specific position</param>
        /// <returns><seealso cref="IEnumerable{AuctionDTO}"/> of <seealso cref="AuctionDTO"/> that was selected with provided params</returns>
        public async Task<IEnumerable<AuctionDTO>> GetAllAsync(string? filters, int? limit, int? offset)
        {
            var limitVal = limit == null || limit > 20 ? 20 : limit.Value;
            var offsetVal = offset ?? 0;

            var auctionQuery = _unitOfWork.GetAll<Auction>()
                .Include(a => a.Images)
                .Include(a => a.Creator)
                .Include(a => a.Categories)
                .ThenInclude(x => x.Category);

            if (filters == null)
                return _mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionDTO>>(await auctionQuery.Skip(offsetVal).Take(limitVal).ToListAsync());

            var filtered = auctionQuery.ApplyFilters(filters);
            _logger.LogInformation(filtered.ToSql());
            return _mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionDTO>>(await filtered.Skip(offsetVal).Take(limitVal).ToListAsync());

        }

        /// <summary>
        /// Selecting entity with provided id
        /// </summary>
        /// <param name="id">id of entity to find</param>
        /// <returns>Found entity mapped to <seealso cref="AuctionDTO"/></returns>
        /// <exception cref="ItemNotFountException">throws when entity with provided id not present in database</exception>
        public async Task<AuctionDTO> GetByIdAsync(Guid id)
        {
            var auction = await GetAuctionAsync(id).ConfigureAwait(false);

            return _mapper.Map<Auction, AuctionDTO>(auction);
        }

        public async Task AttachMediaAsync(Guid auctionId, Guid userId, string[] mediaUrls)
        {
            var auction = await GetAuctionAsync(auctionId);

            if (auction.UserId != userId.ToString())
            {
                throw new AccessViolationException("Unable to update item created by another user");
            }

            auction.Images = mediaUrls.Select(mediaUrl => new Media(mediaUrl)).ToList();

            _unitOfWork.Update(auction);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public async Task<AuctionDTO> UpdateAuctionAsync(AuctionDTO newItem)
        {
            await Task.Delay(-1);
            return null;
        }

        /// <summary>
        /// Deletes entity with provided id
        /// </summary>
        /// <param name="id">id of entity to be deleted</param>
        /// <returns></returns>
        public async Task RemoveAuctionAsync(Guid id)
        {
            var auction = await GetAuctionAsync(id);

            if (auction.StartTime <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                throw new Exception("Can't delete auction that already in action, for additional information contact administration");
            }

            _unitOfWork.Remove(auction);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auctionId"></param>
        /// <param name="userId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public async Task<BidDTO> InsertBidAsync(Guid auctionId, Guid userId, decimal price)
        {
            var auction = await GetAuctionAsync(auctionId).ConfigureAwait(false);

            // TODO: Implement own exception
            if(auction.StartTime > DateTimeOffset.UtcNow.ToUnixTimeSeconds() && auction.EndTime < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                throw new Exception("Auction already expires or not started yet");
            }

            if (auction.UserId == userId.ToString())
            {
                throw new AccessViolationException("Unable to place bids in own auction");
            }

            var latestBids = await _unitOfWork.Find<Bid>(b => b.AuctionId == auctionId)
                                            .OrderBy(a => a.Time)
                                            .Take(1)
                                            .ToListAsync();

            if (latestBids.Any())
            {
                var latestBid = latestBids.First();
                if (latestBid.Price > price && Math.Abs(latestBid.Price - price) < auction.PriceStep)
                {
                    throw new UserException(200, "Price can't be less then latest price with price step");
                }
            }
            else
            {
                if (Math.Abs(auction.StartPrice - price) < auction.PriceStep)
                {
                    throw new UserException(200, "Price can't be less then start price with price step");
                }
            }

            var bid = await _unitOfWork.InsertAsync(new Bid(auctionId, userId, price));
            await _unitOfWork.CommitAsync();

            return _mapper.Map<Bid, BidDTO>(bid); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Auction> GetAuctionAsync(Guid id)
        {
            var auctions = await _unitOfWork.GetAll<Auction>()
                .Where(a => a.Id == id)
                .Include(a => a.Images)
                .Include(a => a.Creator)
                .Include(a => a.Categories)
                .ThenInclude(x => x.Category)
                .ToArrayAsync();

            if (!auctions.Any())
            {
                throw new ItemNotFountException(nameof(auctions), "Auction with following id not found");
            }

            return auctions.First();
        }
    }
}
