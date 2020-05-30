using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class BalanceConfiguration : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.ToTable("Balances")
                .HasIndex(a => a.Id)
                .IsUnique();

            builder.HasOne(bc => bc.User)
                .WithOne(b => b.Balance)
                .HasForeignKey<Balance>(a => a.UserId);
        }
    }
}
