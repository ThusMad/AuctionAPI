using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Services.DataTransferObjects.Objects
{
    public class ApplicationUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("about")]
        public string? About { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("balance")]
        public BalanceDTO Balance { get; set; }
        [JsonPropertyName("bages")]
        public ICollection<BageDTO> Bages { get; set; }
        [JsonPropertyName("registrationDate")]
        public long? RegistrationDate { get; set; }
    }
}
