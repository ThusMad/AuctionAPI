using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace EPAM_BusinessLogicLayer.DTO
{
    public class AuctionDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("startPrice")]
        public decimal StartPrice { get; set; }
        [JsonPropertyName("priceStep")]
        public decimal PriceStep { get; set; }
        [JsonPropertyName("creationTime")]
        public long? CreationTime { get; set; }
        [JsonPropertyName("startTime")]
        public long StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long? EndTime { get; set; }
        [JsonPropertyName("type")]
        public AuctionType AuctionType { get; set; }
        [JsonPropertyName("creator")]
        public ApplicationUserDto? Creator { get; set; }
        [JsonPropertyName("images")]
        public ICollection<string> Images { get; set; }

    }
}
