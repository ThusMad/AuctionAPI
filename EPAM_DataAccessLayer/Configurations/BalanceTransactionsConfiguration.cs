using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    public class BalanceTransactionsConfiguration : IEntityTypeConfiguration<BalanceTransaction>
    {
        public void Configure(EntityTypeBuilder<BalanceTransaction> builder)
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