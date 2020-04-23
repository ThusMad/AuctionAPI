using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPAM_DataAccessLayer.Entities
{
    public class Media
    {
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        public Media(string url)
        {
            Url = url;
        }
    }
}
