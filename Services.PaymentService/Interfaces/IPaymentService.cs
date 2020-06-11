using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.PaymentService.Interfaces
{
    public interface IPaymentService
    {
        Task DeletePaymentMethodAsync(Guid id, Guid userId);
        Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodsAsync(Guid userId);
        Task<PaymentMethodDTO> GetPaymentMethodAsync(Guid methodId, Guid userId);
        Task<PaymentMethodDTO> GetDefaultPaymentMethodAsync(Guid userId);
        Task<PaymentMethodDTO> InsertPaymentMethodAsync(Guid userId, PaymentMethodDTO paymentMethod);
        Task SetDefaultPaymentMethodAsync(Guid userId, Guid paymentMethodId);
        Task<PaymentDTO> GetPaymentAsync(Guid id);
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync(int? limit, int? offset);
        Task InsertAuctionPaymentAsync(Guid userId, Guid auctionId);
    }
}