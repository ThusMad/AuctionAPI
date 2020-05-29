using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EPAM_DataAccessLayer.Enums;

namespace EPAM_DataAccessLayer.Entities
{
    public class Payment
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
        public string? RecipientId { get; set; }
        public ApplicationUser? Recipient { get; set; }

        public Payment()
        {

        }

        public Payment(string senderId, string recipientId, decimal amount, string description)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Amount = amount;
            Description = description;
            Type = PaymentType.Transfer;
            Status = PaymentStatus.Pending;
            TimeOfCreation = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
