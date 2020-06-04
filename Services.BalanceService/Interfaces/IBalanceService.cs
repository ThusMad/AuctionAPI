using System;
using System.Threading.Tasks;

namespace Services.BalanceService.Interfaces
{
    /// <summary>
    /// A service that provides methods to work with user balances.
    /// </summary>
    public interface IBalanceService
    {
        /// <summary>
        /// re
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipientId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task Transfer(Guid userId, Guid recipientId, decimal amount);
        Task Transfer(Guid userId, Guid paymentId);
        Task ReplenishBalance(Guid userId, Guid paymentMethod);
        Task WithdrawalBalance(Guid userId);
        Task WithdrawalBalance(Guid userId, Guid paymentMethod);
        Task WithdrawalBalance(Guid userId, string cardNumber);
    }
}