using EPAM_DataAccessLayer.Enums;
using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class BalanceTransaction
    {
        public Guid Id { get; set; }
        public Balance Balance { get; set; }
        public Guid BalanceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }

        public BalanceTransaction()
        {

        }

        public BalanceTransaction(TransactionType transactionType, Guid balanceId, decimal amount, string description)
        {
            TransactionType = transactionType;
            BalanceId = balanceId;
            Amount = amount;
            Description = description;
        }
    }
}
