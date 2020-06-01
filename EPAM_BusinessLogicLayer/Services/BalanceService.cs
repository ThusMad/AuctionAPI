using System;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Entities.Interfaces.Payments;
using EPAM_DataAccessLayer.Entities.Interfaces.Users;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using AccessViolationException = EPAM_BusinessLogicLayer.Infrastructure.AccessViolationException;

namespace EPAM_BusinessLogicLayer.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BalanceService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public Task Transfer(Guid userId, Guid recipientId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task Transfer(Guid userId, Guid paymentId)
        {
            var payment = await GetPayment(paymentId);
            var sender = await GetUserByIdAsync(userId) as IBalanceHolder;

            if (payment.SenderId != userId.ToString())
            {
                throw new AccessViolationException("Unable to access this payment");
            }

            if (payment.Status == PaymentStatus.Confirmed)
            {
                throw new PaymentAlreadyCompletedException("Payment with following id already completed", paymentId);
            }

            if (sender.Balance.PersonalFunds < payment.Amount)
            {
                throw new NotEnoughFundsException("Insufficient funds to complete the transaction", sender.Balance.PersonalFunds, payment.Amount);
            }

            if (payment.Type == PaymentType.Transfer)
            {
                var recipient = await GetUserByIdAsync(Guid.Parse(payment.RecipientId));

                await HandleTransfer(payment, sender, recipient);
            }
            else
            {
                await HandleSubscription(payment);
            }
        }

        public Task ReplenishBalance(Guid userId, Guid paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Task WithdrawalBalance(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task WithdrawalBalance(Guid userId, Guid paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Task WithdrawalBalance(Guid userId, string cardNumber)
        {
            throw new NotImplementedException();
        }

        private async Task HandleTransfer(ITransferPayment payment, IBalanceHolder senderBalanceHolder, IBalanceHolder recipientBalanceHolder)
        {
            var senderBalance = senderBalanceHolder.Balance;
            var recipientBalance = recipientBalanceHolder.Balance;

            using var transaction = _unitOfWork.BeginTransaction();
                senderBalance.PersonalFunds -= payment.Amount;
                recipientBalance.PersonalFunds += payment.Amount;

                transaction.Update(senderBalance);
                transaction.Update(recipientBalance);

                await transaction.InsertAsync(new BalanceTransactions(TransactionType.Withdrawal, senderBalance.Id,
                    payment.Amount));
                await transaction.InsertAsync(new BalanceTransactions(TransactionType.Refill, recipientBalance.Id, 
                    payment.Amount));
        }

        private async Task HandleSubscription(ISubscriptionPayment payment)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {

            }
        }

        private async Task<Payment> GetPayment(Guid id)
        {
            var payment = await _unitOfWork.GetByIdAsync<Payment>(id).ConfigureAwait(false);

            if (payment == null)
            {
                throw new ItemNotFountException("User", $"User with following {nameof(payment)} not found");
            }

            return payment;
        }

        private async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new ItemNotFountException("User", $"User with following {nameof(id)} not found");
            }

            return user;
        }
    }
}