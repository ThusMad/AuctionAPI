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
        [JsonPropertyName("balanceId")]
        public Guid BalanceId { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("transactionType")]
        public TransactionType TransactionType { get; set; }
    }
}