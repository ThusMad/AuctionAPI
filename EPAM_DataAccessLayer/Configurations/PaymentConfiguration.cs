using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments")
                .HasOne(n => n.Sender)
                .WithMany(n => n.OutgoingPayments)
                .HasForeignKey(n => n.SenderId);

            builder.HasOne(n => n.Recipient)
                .WithMany(n => n.InnerPayments)
                .HasForeignKey(n => n.RecipientId);

            builder.HasIndex(a => a.Id)
                .IsUnique();
        }
    }
}
