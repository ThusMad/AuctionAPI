using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EPAM_BusinessLogicLayer.DataTransferObjects
{
    public class ApplicationUserPatchModel
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("about")]
        public string? About { get; set; }
    }
}
