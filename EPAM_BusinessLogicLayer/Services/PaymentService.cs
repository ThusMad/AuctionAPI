using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AccessViolationException = EPAM_BusinessLogicLayer.Infrastructure.AccessViolationException;

namespace EPAM_BusinessLogicLayer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task DeletePaymentMethodAsync(Guid id, Guid userId)
        {
            var paymentMethod =  await GetById(id);

            if (paymentMethod.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            _unitOfWork.Delete(paymentMethod);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PaymentMethodDTO> GetPaymentMethodAsync(Guid methodId, Guid userId)
        {
            var paymentMethod = await GetById(methodId);

            if (paymentMethod.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            return _mapper.Map<PaymentMethod, PaymentMethodDTO>(paymentMethod);
        }

        public async Task<PaymentMethodDTO> GetDefaultPaymentMethodAsync(Guid userId)
        {
            var defaultPaymentMethods = await _unitOfWork.Find<DefaultPaymentMethod>( m => m.UserId == userId.ToString()).Include(a => a.PaymentMethod).ToListAsync();

            if (defaultPaymentMethods.Any())
            {
                throw new ItemNotFountException(nameof(defaultPaymentMethods), "The user with the following id doesn't have a default payment method");
            }

            return _mapper.Map<PaymentMethod, PaymentMethodDTO>(defaultPaymentMethods.First().PaymentMethod);
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodsAsync(Guid userId)
        {
            var methods = await _unitOfWork.Find<PaymentMethod>(a => a.UserId == userId.ToString()).ToListAsync();

            return _mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodDTO>>(methods);
        }

        public async Task<PaymentMethodDTO> InsertPaymentMethodAsync(Guid userId, PaymentMethodDTO paymentMethod)
        {
            if (_unitOfWork.Find<PaymentMethod>(m => m.CardNumber == paymentMethod.CardNumber) != null)
            {
                throw new ItemExistsException(nameof(paymentMethod),
                    "Card with this card number already exists in database");
            }

            var entity = _mapper.Map<PaymentMethodDTO, PaymentMethod>(paymentMethod, opt => opt.AfterMap((src, dest) =>
                {
                    dest.UserId = userId.ToString();
                }));

            var inserted = await _unitOfWork.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PaymentMethod, PaymentMethodDTO>(inserted);
        }

        public async Task SetDefaultPaymentMethodAsync(Guid userId, Guid paymentMethodId)
        {
            var method = await GetById(paymentMethodId);

            if (method.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            var currentDefault = await _unitOfWork.Find<DefaultPaymentMethod>(dm => dm.UserId == userId.ToString()).ToListAsync();
            var methodToDefault = _mapper.Map<PaymentMethod, DefaultPaymentMethod>(method);

            if (currentDefault.Any())
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    transaction.Delete(currentDefault.First());
                    await transaction.InsertAsync(methodToDefault);
                }
            }
            else
            {
                await _unitOfWork.InsertAsync(methodToDefault);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task InsertAuctionPaymentAsync(Guid userId, Guid auctionId)
        {
            var auction = await _unitOfWork.GetByIdAsync<Auction>(auctionId);

            if (auction == null)
            {
                throw new ItemNotFountException(nameof(auction), "auction method with following id not found");
            }

            if (auction.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot request payment for someone else’s auction");
            }

            if (auction.EndTime == null && auction.EndTime > Utility.DateTimeToUnixTimestamp(DateTime.UtcNow))
            {
                throw new UserException(200, "Unable to request payment at unfinished auction");
            }

            var bids = await _unitOfWork.Find<Bid>(b => b.AuctionId == auction.Id).OrderBy(a => a.Price).ToListAsync();

            if (!bids.Any())
            {
                throw new UserException(200, "There is no bid in the auction, it is impossible to determine the winner");
            }

            var winner = await _userManager.FindByIdAsync(bids.First().PlacerId);
            var roles = await _userManager.GetRolesAsync(winner);

            var payment = new Payment(winner.Id, auction.UserId, new Fee(bids.First().Price).GetFeePrice(roles), auction.Title);

            await _unitOfWork.InsertAsync(payment);
            await _unitOfWork.CommitAsync();
        }

        private async Task<PaymentMethod> GetById(Guid id)
        {
            var method = await _unitOfWork.GetByIdAsync<PaymentMethod>(id);

            if (method == null)
            {
                throw new ItemNotFountException(nameof(method), "payment method with following id not found");
            }

            return method;
        }
    }
}
