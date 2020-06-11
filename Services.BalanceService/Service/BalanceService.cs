using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Entities.Interfaces.Payments;
using EPAM_DataAccessLayer.Entities.Interfaces.Users;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Services.BalanceService.Interfaces;
using Services.DataTransferObjects.Objects;
using Services.Infrastructure.Exceptions;
using AccessViolationException = Services.Infrastructure.Exceptions.AccessViolationException;

namespace Services.BalanceService.Service
{
    //TODO: Add User Updates stream
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

        public async Task ProceedPaymentAsync(Guid userId, Guid paymentId)
        {
            var payment = await GetPaymentByIdAsync(paymentId);
            var sender = await GetUserByIdAsync(userId) as IBalanceHolder;

            if (payment.SenderId != userId.ToString())
            {
                throw new AccessViolationException("Unable to access this payment");
            }

            if (payment.Status == PaymentStatus.Confirmed)
            {
                throw new PaymentAlreadyCompletedException("Payment with following userId already completed", paymentId);
            }

            if (sender.Balance.PersonalFunds < payment.Amount)
            {
                throw new NotEnoughFundsException("Insufficient funds to complete the transaction", sender.Balance.PersonalFunds, payment.Amount);
            }

            if (payment.Type == PaymentType.Transfer)
            {
                var recipient = await GetUserByIdAsync(Guid.Parse(payment.RecipientId));

                await HandleTransferAsync(payment, sender, recipient);
            }
            else
            {
                await HandleSubscriptionAsync(payment, sender);
            }
        }

        public async Task RefillBalanceAsync(Guid userId, Guid paymentMethodId, decimal amount)
        {
            var paymentMethod = await GetPaymentMethodByIdAsync(paymentMethodId);
            var balance = await GetUserBalanceByIdAsync(userId);

            using var transaction = _unitOfWork.BeginTransaction();

            balance.PersonalFunds += amount;
            transaction.Update(balance);
            await transaction.InsertAsync(new BalanceTransaction(TransactionType.Refill, balance.Id,
                amount, $"From the card {paymentMethod.CardNumber}"));
        }

        public async Task WithdrawalBalanceAsync(Guid userId, Guid paymentMethodId, decimal amount)
        {
            var paymentMethod = await GetPaymentMethodByIdAsync(paymentMethodId);
            var balance = await GetUserBalanceByIdAsync(userId);

            using var transaction = _unitOfWork.BeginTransaction();

            balance.PersonalFunds -= amount;
            transaction.Update(balance);
            await transaction.InsertAsync(new BalanceTransaction(TransactionType.Withdrawal, balance.Id,
                amount, $"To the card {paymentMethod.CardNumber}"));
        }

        public async Task WithdrawalBalanceAsync(Guid userId, string cardNumber, decimal amount)
        {
            var balance = await GetUserBalanceByIdAsync(userId);

            using var transaction = _unitOfWork.BeginTransaction();

            balance.PersonalFunds -= amount;
            transaction.Update(balance);
            await transaction.InsertAsync(new BalanceTransaction(TransactionType.Withdrawal, balance.Id,
                amount, $"To: {cardNumber}"));
        }

        public async Task<BalanceTransactionDTO> GetBalanceTransactionAsync(Guid userId, Guid transactionId)
        {
            var transaction = await GetBalanceTransactionByIdAsync(transactionId);
            var senderBalance = await GetUserBalanceByIdAsync(transaction.BalanceId);

            if (senderBalance.UserId != userId.ToString())
            {
                throw new AccessViolationException("Unable to access this transaction");
            }

            return _mapper.Map<BalanceTransaction, BalanceTransactionDTO>(transaction);
        }

        public async Task<IEnumerable<BalanceTransactionDTO>> GetAllBalanceTransactionsAsync(Guid userId, int? limit, int? offset)
        {
            var limitVal = limit == null || limit > 20 ? 20 : limit.Value;
            var offsetVal = offset ?? 0;

            var user = await GetBalanceTransactionByIdAsync(userId);

            var transactionsQuery = _unitOfWork.Find<BalanceTransaction>(t => t.BalanceId == user.BalanceId)
                .Skip(offsetVal)
                .Take(limitVal);

            return _mapper.Map<IEnumerable<BalanceTransaction>, IEnumerable<BalanceTransactionDTO>>(await transactionsQuery.ToListAsync());
        }


        private async Task HandleTransferAsync(ITransferPayment payment, IBalanceHolder senderBalanceHolder, IBalanceHolder recipientBalanceHolder)
        {
            var senderBalance = senderBalanceHolder.Balance;
            var recipientBalance = recipientBalanceHolder.Balance;

            using var transaction = _unitOfWork.BeginTransaction();
                senderBalance.PersonalFunds -= payment.Amount;
                recipientBalance.PersonalFunds += payment.Amount;

                transaction.Update(senderBalance);
                transaction.Update(recipientBalance);

                await transaction.InsertAsync(new BalanceTransaction(TransactionType.Withdrawal, senderBalance.Id,
                    payment.Amount, payment.Description));
                await transaction.InsertAsync(new BalanceTransaction(TransactionType.Refill, recipientBalance.Id, 
                    payment.Amount, payment.Description));
        }

        private async Task HandleSubscriptionAsync(ISubscriptionPayment payment, IBalanceHolder senderBalanceHolder)
        {
            var senderBalance = senderBalanceHolder.Balance;

            using var transaction = _unitOfWork.BeginTransaction();
                senderBalance.PersonalFunds -= payment.Amount;
                transaction.Update(senderBalance);

                await transaction.InsertAsync(new BalanceTransaction(TransactionType.Withdrawal, senderBalance.Id,
                    payment.Amount, payment.Description));
        }

        private async Task<BalanceTransaction> GetBalanceTransactionByIdAsync(Guid id)
        {
            var balanceTransaction = await _unitOfWork.GetByIdAsync<BalanceTransaction>(id).ConfigureAwait(false);

            if (balanceTransaction == null)
            {
                throw new ItemNotFountException("BalanceTransaction", $"Balance transaction with following {nameof(id)} not found");
            }

            return balanceTransaction;
        }

        private async Task<PaymentMethod> GetPaymentMethodByIdAsync(Guid id)
        {
            var paymentMethod = await _unitOfWork.GetByIdAsync<PaymentMethod>(id).ConfigureAwait(false);

            if (paymentMethod == null)
            {
                throw new ItemNotFountException("PaymentMethod", $"PaymentMethod with following {nameof(id)} not found");
            }

            return paymentMethod;
        }

        private async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _unitOfWork.GetByIdAsync<Payment>(id).ConfigureAwait(false);

            if (payment == null)
            {
                throw new ItemNotFountException("User", $"Payment with following {nameof(id)} not found");
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

        private async Task<Balance> GetUserBalanceByIdAsync(Guid userId)
        {
            var balance = _unitOfWork.Find<Balance>(b => b.UserId == userId.ToString());
            var balances = await balance.ToListAsync();
            if (!balances.Any())
            {
                throw new ItemNotFountException("User", $"User with following {nameof(userId)} not found");
            }

            return balances.First();
        }
    }
}