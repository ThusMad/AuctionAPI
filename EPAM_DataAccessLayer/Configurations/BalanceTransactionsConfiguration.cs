using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    public class BalanceTransactionsConfiguration : IEntityTypeConfiguration<BalanceTransactions>
    {
        public void Configure(EntityTypeBuilder<BalanceTransactions> builder)
        {
            builder.ToTable("BalanceTransactions")
                .HasIndex(a => a.Id)
                .IsUnique();

            builder.HasOne(bc => bc.Balance)
                .WithMany(b => b.Transactions)
                .HasForeignKey(bc => bc.BalanceId);
        }
    }
}