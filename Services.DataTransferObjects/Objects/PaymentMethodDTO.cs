using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Services.DataTransferObjects.Objects
{
    public class PaymentMethodDTO
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }
        [JsonPropertyName("cardNumber")]
        [Required(ErrorMessage = "An card_number is required"),
         RegularExpression(@"\b4[0-9]{12}(?:[0-9]{3})?\b|\b5[1-5][0-9]{14}\b|\b3[47][0-9]{13}\b|\b3(?:0[0-5]|[68][0-9])[0-9]{11}\b|\b6(?:011|5[0-9]{2})[0-9]{12}\b|\b(?:2131|1800|35\d{3})\d{11}\b",
             ErrorMessage =
                 "card number not matching pattern")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "An card_holder is required")]
        [JsonPropertyName("cardHolder")]
        public string Cardholder { get; set; }
        [JsonPropertyName("expiration")]
        [Required(ErrorMessage = "An expiration is required")]
        public long ExpirationDate { get; set; }
    }
}
