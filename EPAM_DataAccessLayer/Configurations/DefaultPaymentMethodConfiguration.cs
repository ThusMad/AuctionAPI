using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class DefaultPaymentMethodConfiguration : IEntityTypeConfiguration<DefaultPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<DefaultPaymentMethod> builder)
        {
            builder.ToTable("DefaultPaymentMethods")
                .HasOne(b => b.User)
                .WithOne(a => a.DefaultPaymentMethod)
                .HasForeignKey<DefaultPaymentMethod>(b => b.UserId);

            builder.HasOne(r => r.PaymentMethod)   
                .WithMany()
                .HasForeignKey(a => a.PaymentMethodId);
        }
    }
}
