using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("Auctions")
                .HasOne(b => b.Creator)
                .WithMany(a => a.Auctions)
                .HasForeignKey(b => b.UserId);

            builder.HasMany(b => b.Bids)
                .WithOne(a => a.Auction)
                .HasForeignKey(b => b.AuctionId);

            builder.HasIndex(a => a.Id)
                .IsUnique();
        }
    }
}
