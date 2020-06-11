using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAM_DataAccessLayer.Entities
{
    [Table("Bids")]
    public class Bid
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public long Time { get; set; }

        public Bid()
        {

        }

        public Bid(Guid auctionId, Guid userId, decimal price)
        {
            AuctionId = auctionId;
            UserId = userId.ToString();
            Price = price;
            Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

    }
}
