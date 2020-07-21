using System;
using System.Text.Json.Serialization;

namespace Services.DataTransferObjects.Objects
{
    public class BalanceDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("funds")]
        public decimal PersonalFunds { get; set; }
    }
}