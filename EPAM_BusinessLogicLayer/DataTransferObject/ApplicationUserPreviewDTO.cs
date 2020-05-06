using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.DataTransferObject
{
    public class ApplicationUserPreviewDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("about")]
        public string? About { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("bages")]
        public ICollection<Bage> Bages { get; set; }
    }
}
