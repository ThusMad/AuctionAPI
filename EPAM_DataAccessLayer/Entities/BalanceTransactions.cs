using EPAM_DataAccessLayer.Enums;
using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class BalanceTransactions
    {
        public Guid Id { get; set; }
        public Balance Balance { get; set; }
        public Guid BalanceId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
