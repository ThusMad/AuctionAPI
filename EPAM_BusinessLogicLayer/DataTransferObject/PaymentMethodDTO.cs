using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EPAM_BusinessLogicLayer.DataTransferObject
{
    public class PaymentMethodDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }
        [JsonPropertyName("card_holder")]
        public string Cardholder { get; set; }
        [JsonPropertyName("expiration")]
        public long ExpirationDate { get; set; }
    }
}
