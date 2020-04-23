using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EPAM_DataAccessLayer.Enums;

namespace EPAM_DataAccessLayer.Entities
{
    [Table("Auctions")]
    public class Auction
    {
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal StartPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceStep { get; set; }
        public long CreationTime { get; set; }
        public long StartTime { get; set; }
        public long? EndTime { get; set; }
        public AuctionType AuctionType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser Creator { get; set; }
        public ICollection<Media> Images { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
