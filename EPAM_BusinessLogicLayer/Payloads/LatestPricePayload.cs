using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_BusinessLogicLayer.DTO;

namespace EPAM_BusinessLogicLayer.Payloads
{
    public class LatestPricePayload
    {
        [JsonPropertyName("auctionId")]
        public Guid AuctionId { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        public LatestPricePayload(decimal price, Guid id)
        {
            AuctionId = id;
            Price = price;
        }
    }
}
