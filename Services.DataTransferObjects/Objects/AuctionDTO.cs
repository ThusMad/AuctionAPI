using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;
using Services.DataTransferObjects.Objects;
using Services.Helpers.Validators;

namespace Services.DataTransferObjects.Objects
{
    public class AuctionDTO
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "A title is required"), MinLength(4, ErrorMessage = "Title can't be that short!")]
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A description is required")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "A startPrice is required"), Range(1, double.PositiveInfinity)]
        [JsonPropertyName("startPrice")]
        public decimal StartPrice { get; set; }
        [Required(ErrorMessage = "A priceStep is required"), Range(1, double.PositiveInfinity)]
        [JsonPropertyName("priceStep")]
        public decimal PriceStep { get; set; }
        [JsonPropertyName("creationTime")]
        public long? CreationTime { get; set; }
        [Required(ErrorMessage = "A startTime is required"), TimestampValidator(ErrorMessage = "Start date can't be earlier than current date")]
        [JsonPropertyName("startTime")]
        public long StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long? EndTime { get; set; }
        [Required(ErrorMessage = "A type is required")]
        [JsonPropertyName("type")]
        public AuctionType AuctionType { get; set; }
        [JsonPropertyName("creator")]
        public ApplicationUserPreviewDTO? Creator { get; set; }
        [JsonPropertyName("images")]
        public ICollection<string>? Images { get; set; }
        [Required(ErrorMessage = "A categories is required")]
        [JsonPropertyName("categories")]
        public ICollection<AuctionCategoryDto>? Categories { get; set; }
        
    }
}
