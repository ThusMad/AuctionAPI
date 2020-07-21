using EPAM_DataAccessLayer.Enums;

namespace EPAM_DataAccessLayer.Entities.Interfaces.Payments
{
    public interface ITransferPayment : IPaymentBase
    {
        public string? RecipientId { get; set; }
        public ApplicationUser? Recipient { get; set; }
    }
}