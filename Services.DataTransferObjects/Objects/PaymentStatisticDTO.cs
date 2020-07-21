using System.Text.Json.Serialization;

namespace Services.DataTransferObjects.Objects
{
    public class PaymentStatisticDTO
    {
        [JsonPropertyName("awaiting")]
        public int Awaiting { get; set; }
        [JsonPropertyName("completed")]
        public int Completed { get; set; }
        [JsonPropertyName("canceled")]
        public int Canceled { get; set; }
        [JsonPropertyName("inProgress")]
        public int InProgress { get; set; }
    }
}