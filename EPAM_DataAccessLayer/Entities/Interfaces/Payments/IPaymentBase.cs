using System;
using EPAM_DataAccessLayer.Enums;

namespace EPAM_DataAccessLayer.Entities.Interfaces.Payments
{
    public interface IPaymentBase
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public long TimeOfCreation { get; set; }
        public long TimeOfPayment { get; set; }
        public string Description { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentType Type { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}