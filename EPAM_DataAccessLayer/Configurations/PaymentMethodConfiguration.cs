using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods")
                .HasOne(n => n.User)
                .WithMany(n => n.PaymentMethods)
                .HasForeignKey(n => n.UserId);
        }
    }
}
