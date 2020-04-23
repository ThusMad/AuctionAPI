using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPAM_DataAccessLayer.Entities
{
    public class DefaultPaymentMethod
    {
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; }
    }
}
