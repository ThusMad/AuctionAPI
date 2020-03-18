using System.Data.Entity;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_DataAccessLayer.EF
{
    public class AuctionContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        static AuctionContext()
        {
            Database.SetInitializer<AuctionContext>(new AuctionDbInitializer());
        }
        public AuctionContext(string connectionString)
            : base(connectionString)
        {
        }
    }

    public class AuctionDbInitializer : DropCreateDatabaseIfModelChanges<AuctionContext>
    {
        protected override void Seed(AuctionContext db)
        {
            //db.Users.Add(new Phone { Name = "Nokia Lumia 630", Company = "Nokia", Price = 220 });
            //db.Users.Add(new Phone { Name = "iPhone 6", Company = "Apple", Price = 320 });
            //db.Users.Add(new Phone { Name = "LG G4", Company = "lG", Price = 260 });
            //db.Users.Add(new Phone { Name = "Samsung Galaxy S 6", Company = "Samsung", Price = 300 });
            db.SaveChanges();
        }
    }
}
