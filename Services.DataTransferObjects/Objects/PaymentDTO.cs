using System;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;

namespace Services.DataTransferObjects.Objects
{
    public class PaymentDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("creationTime")]
        public long TimeOfCreation { get; set; }
        [JsonPropertyName("paymentTime")]
        public long TimeOfPayment { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("paymentStatus")]
        public PaymentStatus Status { get; set; }
        [JsonPropertyName("paymentType")]
        public PaymentType Type { get; set; }
        [JsonPropertyName("senderId")]
        public string SenderId { get; set; }
        [JsonPropertyName("recipientId")]
        public string? RecipientId { get; set; }
    }
}