using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EPAM_BusinessLogicLayer.DTO
{
    public class UserDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
