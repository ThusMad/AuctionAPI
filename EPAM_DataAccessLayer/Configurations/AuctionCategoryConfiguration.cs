using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class AuctionCategoryConfiguration : IEntityTypeConfiguration<AuctionCategory>
    {
        public void Configure(EntityTypeBuilder<AuctionCategory> builder)
        {
            builder.ToTable("AuctionCategories")
                .HasKey(bc => new
                {
                    bc.AuctionId,
                    bc.CategoryId
                });
            builder.HasOne(bc => bc.Auction)
                .WithMany(b => b.Categories)
                .HasForeignKey(bc => bc.AuctionId);

            builder.HasOne(bc => bc.Category)
                .WithMany(b => b.Auctions)
                .HasForeignKey(bc => bc.CategoryId);
        }
    }
}
