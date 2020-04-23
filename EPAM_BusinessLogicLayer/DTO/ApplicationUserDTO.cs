using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.DTO
{
    public class ApplicationUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("registrationDate")]
        public long? RegistrationDate { get; set; }
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonPropertyName("bages")]
        public ICollection<Bage> Bages { get; set; }

        public ApplicationUserDto()
        {
            Bages = new List<Bage>();
        }
    }
}
