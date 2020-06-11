using System;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;

namespace Services.DataTransferObjects.Objects
{
    public class BalanceTransactionDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("balance_id")]
        public Guid BalanceId { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("transaction_type")]
        public TransactionType TransactionType { get; set; }
    }
}