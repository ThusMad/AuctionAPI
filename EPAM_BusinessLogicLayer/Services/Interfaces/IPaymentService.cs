using System;
using System.Collections.Generic;
using EPAM_BusinessLogicLayer.DataTransferObject;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IPaymentService
    {
        void DeletePaymentMethod(Guid id);
        IEnumerable<PaymentMethodDTO>? GetPaymentMethods(Guid userId);
        void AddPaymentMethod(Guid userId, PaymentMethodDTO paymentMethod);
    }
}