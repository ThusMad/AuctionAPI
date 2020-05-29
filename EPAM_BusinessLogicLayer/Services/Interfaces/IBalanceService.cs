using System;
using System.Threading.Tasks;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IBalanceService
    {
        Task ReplenishBalance(Guid userId);
        Task ReplenishBalance(Guid userId, Guid paymentMethod);
        Task WithdrawalBalance(Guid userId);
        Task WithdrawalBalance(Guid userId, Guid paymentMethod);
        Task WithdrawalBalance(Guid userId, string cardNumber);
    }
}