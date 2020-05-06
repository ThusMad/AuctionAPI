using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels.SocketSlot;
using EPAM_BusinessLogicLayer.BusinessModels.SocketSlot.Interfaces;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Payloads;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPAM_BusinessLogicLayer.Services
{
    class AuctionService : IAuctionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ISocketSlot _socketSlot;

        public AuctionService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _socketSlot = new SocketSlot();

            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuctionDTO> CreateAuction(AuctionDTO auctionDto, Guid userId, string userRole)
        {
            if (auctionDto.StartTime < auctionDto.EndTime && auctionDto.AuctionType == AuctionType.Normal)
            {
                throw new ValidationException("End date can't be ahead of the start date", nameof(auctionDto));
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

        public IEnumerable<AuctionDTO> GetAll(int? limit, int? offset)
        {
            var limitVal = limit == null || limit > 20 ? 20 : limit.Value;
            var offsetVal = offset ?? 0;

            var auctions = _unitOfWork.GetAll<Auction>(limitVal, offsetVal)
                .Include(a => a.Images)
                .Include(a => a.Creator)
                .Include(a => a.Categories)
                    .ThenInclude(x => x.Category)
                .AsEnumerable();

            return _mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionDTO>>(auctions);
        }

        public AuctionDTO GetById(Guid id)
        {
            var auction = _unitOfWork.GetById<Auction>(id);

            if (auction == null)
            {
                throw new ItemNotFountException(nameof(auction), "Auction with following id not found");
            }

            return _mapper.Map<Auction, AuctionDTO>(auction);
        }

        public AuctionDTO Update(AuctionDTO newItem)
        {
            return null;
        }

        public void DeleteAuction(Guid id)
        {
            throw new NotImplementedException();
        }

        public void PlaceBid(BidDTO bid)
        {
            _socketSlot.NotifyAllSubscribers(bid.AuctionId, JsonSerializer.Serialize(bid));
        }

        public LatestPricePayload CurrentPrice(Guid id)
        {
            return new LatestPricePayload(10, id);
        }

        public IEnumerable<AuctionCategoryDto> GetCategories(int? limit, int? offset)
        {
            var categories = _unitOfWork.GetAll<Category>().AsEnumerable();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<AuctionCategoryDto>>(categories);
        }

        public async Task AddCategoriesAsync(IEnumerable<AuctionCategoryDto> categories)
        {
            var newCategories = new List<AuctionCategoryDto>();

            foreach (var auctionCategoryDto in categories)
            {
                if (!await _unitOfWork.AnyAsync<Category>(c => c.Name == auctionCategoryDto.Name))
                {
                    newCategories.Add(auctionCategoryDto);
                }
            }

            _mapper.Map<IEnumerable<AuctionCategoryDto>, IEnumerable<Category>>(newCategories)
                .ToList()
                .ForEach(async a => await _unitOfWork.InsertAsync(a));

            await _unitOfWork.CommitAsync();
        }

        #region ISlotProvider

        public void SubscribeToSlot(Guid id, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs)
        {
            _socketSlot.SubscribeToSlot(id, ws, socketFinishedTcs);
        }

        #endregion
    }
}
