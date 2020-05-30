using System;
using System.Collections.Generic;

namespace EPAM_DataAccessLayer.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AuctionCategory>? Auctions { get; set; }
    }
}
