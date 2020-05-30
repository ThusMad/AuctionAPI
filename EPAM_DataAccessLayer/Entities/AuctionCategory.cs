using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class AuctionCategory
    {
        public Guid AuctionId { get; set; }
        public Auction? Auction { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
