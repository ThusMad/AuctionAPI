using System.Text.Json.Serialization;

namespace Services.DataTransferObjects.Objects
{
    public class ParticipantDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("profilePicture")]
        public string? ProfilePicture { get; set; }
    }
}