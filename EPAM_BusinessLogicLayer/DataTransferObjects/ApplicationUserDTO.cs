using System.Text.Json.Serialization;

namespace EPAM_BusinessLogicLayer.DataTransferObjects
{
    public class ApplicationUserDto : ApplicationUserPreviewDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("registrationDate")]
        public long? RegistrationDate { get; set; }
    }
}
