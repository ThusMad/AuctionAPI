using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.BalanceService.Interfaces
{
    /// <summary>
    /// A service that provides methods to work with user balances.
    /// </summary>
    public interface IBalanceService
    {
        Task ProceedPaymentAsync(Guid userId, Guid paymentId);
        Task RefillBalanceAsync(Guid userId, Guid paymentMethodId, decimal amount);
        Task WithdrawalBalanceAsync(Guid userId, Guid paymentMethod, decimal amount);
        Task WithdrawalBalanceAsync(Guid userId, string cardNumber, decimal amount);
        Task<BalanceTransactionDTO> GetBalanceTransactionAsync(Guid userId, Guid transactionId);
        Task<IEnumerable<BalanceTransactionDTO>> GetAllBalanceTransactionsAsync(Guid userId, int? limit, int? offset);
    }
}