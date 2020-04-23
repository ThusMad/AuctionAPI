using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EPAM_BusinessLogicLayer.DTO
{
    public class BidDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        public Guid? UserId { get; set; }
        [JsonPropertyName("auctionId")]
        public Guid AuctionId { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }
    }
}
