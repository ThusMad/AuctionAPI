using EPAM_DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAM_DataAccessLayer.Entities
{
    [Table("Auctions")]
    public class Auction
    {
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
        public AuctionType Type { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser? Creator { get; set; }
        public virtual ICollection<Media>? Images { get; set; }
        public virtual ICollection<Bid>? Bids { get; set; }
        public virtual ICollection<AuctionCategory>? Categories { get; set; }
    }
}
