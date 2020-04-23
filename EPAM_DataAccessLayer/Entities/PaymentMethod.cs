using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPAM_DataAccessLayer.Entities
{
    public class PaymentMethod
    {
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string Cardholder { get; set; }
        public long ExpirationDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
