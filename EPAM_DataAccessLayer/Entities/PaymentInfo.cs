using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EPAM_DataAccessLayer.Enums;

namespace EPAM_DataAccessLayer.Entities
{
    public class PaymentInfo
    {
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public long Date { get; set; }
        public string Name { get; set; }
        public PaymentStatus Status { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
