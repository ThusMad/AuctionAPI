using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class PaymentAlreadyCompletedException : ErrorException
    {
        public Guid PaymentId { get; set; }

        public PaymentAlreadyCompletedException(string msg, Guid paymentId) : base(msg)
        {
            PaymentId = paymentId;
        }
    }
}
