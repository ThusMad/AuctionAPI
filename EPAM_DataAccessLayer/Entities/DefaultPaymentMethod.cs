using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class DefaultPaymentMethod
    {
        public Guid? PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
