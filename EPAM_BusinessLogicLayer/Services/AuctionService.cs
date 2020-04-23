using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Payloads;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    class AuctionService : IAuctionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ConcurrentDictionary<Guid, List<WebSocket>> _bidUpdateSockets;

        public AuctionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _bidUpdateSockets = new ConcurrentDictionary<Guid, List<WebSocket>>();

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public AuctionDTO? CreateAuction(AuctionDTO auctionDto, Guid userId, string userRole)
        {
            if (auctionDto.StartPrice < 1 || auctionDto.PriceStep < 1)
            {
                throw new ValidationException("Start price or price step less then 1 usd", nameof(auctionDto));
            }
            if (auctionDto.Title.Length < 5)
            {
                throw new ValidationException("Title too short", nameof(auctionDto));
            }
            if (auctionDto.StartTime < auctionDto.EndTime && auctionDto.AuctionType == AuctionType.Normal)
            {
                throw new ValidationException("End date can't be ahead of the start date", nameof(auctionDto));
            }

            ICollection<Media> mediaCollection = new List<Media>();
            var creator = _unitOfWork.Repository<ApplicationUser>().GetById(userId);

            foreach (var auctionImage in auctionDto.Images)
            {
                mediaCollection.Add(new Media(auctionImage));
            }

            var auction = _mapper.Map<AuctionDTO, Auction>(auctionDto);

            auction.Images = mediaCollection;
            auction.Creator = creator;
            auction.CreationTime = Utility.DateTimeToUnixTimestamp(DateTime.UtcNow);

            _unitOfWork.Repository<Auction>().Insert(auction);
            _unitOfWork.Commit();

            return _mapper.Map<Auction, AuctionDTO>(auction);
        }

        public void DeleteAuction(Guid id)
        {
            throw new NotImplementedException();
        }

        public void PlaceBid(BidDTO bid)
        {
            //_unitOfWork.Repository<Bid>().Insert();

            if (_bidUpdateSockets.ContainsKey(bid.AuctionId))
            {
                foreach (var socket in _bidUpdateSockets[bid.AuctionId])
                {
                    Task.Run(async () => SendString(socket, "hello", CancellationToken.None));
                }
            }
        }

        public LatestPricePayload CurrentPrice(Guid id)
        {
            var bidRepository = _unitOfWork.Repository<Bid>();

            return new LatestPricePayload(10, id);
        }

        public IEnumerable<AuctionDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionDTO>>(_unitOfWork.Repository<Auction>().GetAll());
        }

        public AuctionDTO? GetAuction(Guid id)
        {
            var repository = _unitOfWork.Repository<Auction>();
            var auction = repository.GetById(id);

            return _mapper.Map<Auction, AuctionDTO>(auction);
        }

        public ICollection<AuctionDTO> Find(Func<bool, AuctionDTO> predicate)
        {
            throw new NotImplementedException();
        }

        public ICollection<AuctionDTO> GetOngoing()
        {
            throw new NotImplementedException();
        }

        public ICollection<AuctionDTO> GetUpcoming()
        {
            throw new NotImplementedException();
        }

        public ICollection<AuctionDTO> GetRandom()
        {
            throw new NotImplementedException();
        }

        public void SubscribeToAuctionBidUpdatesAsync(Guid auctionId, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs)
        {
            bool addResult = true;
            bool getResult = true;

            if (!_bidUpdateSockets.ContainsKey(auctionId))
            {
                while (!_bidUpdateSockets.TryAdd(auctionId, new List<WebSocket>()))
                {
                    Thread.Sleep(10);
                }
            }

            _bidUpdateSockets[auctionId].Add(ws);

            while (ws.State == WebSocketState.Open)
            {
                Thread.Sleep(1000 * 15);
            }

            Debug.WriteLine("closing websocket");

            List<WebSocket> sockets;

            while (!_bidUpdateSockets.TryGetValue(auctionId, out sockets))
            {
                Thread.Sleep(10);
            }

            sockets?.Remove(ws);
            socketFinishedTcs.SetResult(new object());
        }

        public static Task SendString(WebSocket ws, string data, CancellationToken cancellation)
        {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            return ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }
    }
}
