﻿using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string Cardholder { get; set; }
        public long ExpirationDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
