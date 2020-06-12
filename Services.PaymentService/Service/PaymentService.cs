using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.DataTransferObjects.Objects;
using Services.Infrastructure.Exceptions;
using Services.PaymentService.Interfaces;
using AccessViolationException = Services.Infrastructure.Exceptions.AccessViolationException;

namespace Services.PaymentService.Service
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

        #region PaymentMethods

        public async Task DeletePaymentMethodAsync(Guid id, Guid userId)
        {
            var paymentMethod = await GetPaymentMethodByIdAsync(id).ConfigureAwait(false);

            if (paymentMethod.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            _unitOfWork.Remove(paymentMethod);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PaymentMethodDTO> GetPaymentMethodAsync(Guid methodId, Guid userId)
        {
            var paymentMethod = await GetPaymentMethodByIdAsync(methodId).ConfigureAwait(false);

            if (paymentMethod.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            return _mapper.Map<PaymentMethod, PaymentMethodDTO>(paymentMethod);
        }

        public async Task<PaymentMethodDTO> GetDefaultPaymentMethodAsync(Guid userId)
        {
            var defaultPaymentMethods = await _unitOfWork.Find<DefaultPaymentMethod>(m => m.UserId == userId.ToString()).Include(a => a.PaymentMethod).ToListAsync();

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
            var method = await GetPaymentMethodByIdAsync(paymentMethodId).ConfigureAwait(false);

            if (method.UserId != userId.ToString())
            {
                throw new AccessViolationException("Cannot access payment method of another user");
            }

            var currentDefault = await _unitOfWork.Find<DefaultPaymentMethod>(dm => dm.UserId == userId.ToString()).ToListAsync();
            var methodToDefault = _mapper.Map<PaymentMethod, DefaultPaymentMethod>(method);

            if (currentDefault.Any())
            {
                using var transaction = _unitOfWork.BeginTransaction();

                transaction.Remove(currentDefault.First());
                await transaction.InsertAsync(methodToDefault);
            }
            else
            {
                await _unitOfWork.InsertAsync(methodToDefault);
                await _unitOfWork.CommitAsync();
            }
        }

        #endregion

        #region InnerPayments

        public async Task<PaymentDTO> GetPaymentAsync(Guid id)
        {
            var payment = await GetPaymentByIdAsync(id);

            return _mapper.Map<Payment, PaymentDTO>(payment);
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync(int? limit, int? offset)
        {
            var limitVal = limit == null || limit > 20 ? 20 : limit.Value;
            var offsetVal = offset ?? 0;

            var payments = _unitOfWork.GetAll<Payment>(limitVal, offsetVal);

            return _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDTO>>(await payments.ToListAsync());
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

            if (auction.EndTime == null && auction.EndTime > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                throw new UserException(200, "Unable to request payment at unfinished auction");
            }

            var bids = await _unitOfWork.Find<Bid>(b => b.AuctionId == auction.Id).OrderBy(a => a.Price).ToListAsync();

            if (!bids.Any())
            {
                throw new UserException(200, "There is no bid in the auction, it is impossible to determine the winner");
            }

            var winner = await _userManager.FindByIdAsync(bids.First().UserId);
            var roles = await _userManager.GetRolesAsync(winner);

            var payment = new Payment(winner.Id, auction.UserId, new Fee(bids.First().Price).GetFeePrice(roles), auction.Title);

            await _unitOfWork.InsertAsync(payment);
            await _unitOfWork.CommitAsync();
        }

        #endregion

        private async Task<PaymentMethod> GetPaymentMethodByIdAsync(Guid id)
        {
            var method = await _unitOfWork.GetByIdAsync<PaymentMethod>(id);

            if (method == null)
            {
                throw new ItemNotFountException(nameof(method), "Payment method with following id not found");
            }

            return method;
        }

        private async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _unitOfWork.GetByIdAsync<Payment>(id);

            if (payment == null)
            {
                throw new ItemNotFountException(nameof(payment), "Payment with following id not found");
            }

            return payment;
        }
    }
}
