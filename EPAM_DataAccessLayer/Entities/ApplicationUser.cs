using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EPAM_DataAccessLayer.Entities.Interfaces.Users;

namespace EPAM_DataAccessLayer.Entities
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser, IBalanceHolder
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(256)]
        public string? About { get; set; }
        public long? RegistrationDate { get; set; }
        public Balance Balance { get; set; }
        public Media? ProfilePicture { get; set; }
        public DefaultPaymentMethod DefaultPaymentMethod { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<Payment> InnerPayments { get; set; }
        public ICollection<Payment> OutgoingPayments { get; set; }
        public ICollection<Auction> Auctions { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Bage> Bages { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public ApplicationUser()
        {
            Bages = new Collection<Bage>();
            Bids = new Collection<Bid>();
            InnerPayments = new Collection<Payment>();
            Auctions = new Collection<Auction>();
            PaymentMethods = new Collection<PaymentMethod>();
        }
    }
}
