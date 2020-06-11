using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("Bids").HasOne(b => b.User)
                .WithMany(a => a.Bids)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(b => b.UserId);

            builder.HasIndex(a => a.Id)
                .IsUnique();
        }
    }
}
