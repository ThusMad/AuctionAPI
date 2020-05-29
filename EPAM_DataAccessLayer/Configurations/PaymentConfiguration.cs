using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment")
                .HasOne(n => n.Sender)
                .WithMany(n => n.Payments)
                .HasForeignKey(n => n.SenderId);

            builder.HasIndex(a => a.Id)
                .IsUnique();
        }
    }
}
