using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EPAM_DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace EPAM_DataAccessLayer.Entities
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(256)]
        public string? About { get; set; }
        public long? RegistrationDate { get; set; }
        public DefaultPaymentMethod DefaultPaymentMethod { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<PaymentInfo> PaymentInfos { get; set; }
        public ICollection<Auction> Auctions { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Bage> Bages { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public ApplicationUser()
        {
            Bages = new Collection<Bage>();
            Bids = new Collection<Bid>();
            PaymentInfos = new Collection<PaymentInfo>();
            Auctions = new Collection<Auction>();
            PaymentMethods = new Collection<PaymentMethod>();
        }
    }
}
