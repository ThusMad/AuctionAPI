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
        [JsonPropertyName("creation_time")]
        public long TimeOfCreation { get; set; }
        [JsonPropertyName("payment_time")]
        public long TimeOfPayment { get; set; }
        [JsonPropertyName("description_time")]
        public string Description { get; set; }
        [JsonPropertyName("payment_status")]
        public PaymentStatus Status { get; set; }
        [JsonPropertyName("payment_type")]
        public PaymentType Type { get; set; }
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }
        [JsonPropertyName("recipient_id")]
        public string? RecipientId { get; set; }
    }
}