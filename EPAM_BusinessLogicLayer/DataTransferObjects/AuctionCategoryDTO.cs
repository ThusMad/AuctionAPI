using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.DataTransferObjects
{
    public class AuctionCategoryDto 
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
