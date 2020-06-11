using System;
using System.Collections.Generic;

namespace EPAM_DataAccessLayer.Entities
{
    public class Balance
    {
        public Guid Id { get; set; }
        public decimal PersonalFunds { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<BalanceTransaction>? Transactions { get; set; }
    }
}
