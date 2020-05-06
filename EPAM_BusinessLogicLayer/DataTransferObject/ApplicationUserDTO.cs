using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.DataTransferObject
{
    public class ApplicationUserDto : ApplicationUserPreviewDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("registrationDate")]
        public long? RegistrationDate { get; set; }
    }
}
