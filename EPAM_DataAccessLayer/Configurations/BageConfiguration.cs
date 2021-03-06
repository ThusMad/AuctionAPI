﻿using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class BageConfiguration : IEntityTypeConfiguration<Bage>
    {
        public void Configure(EntityTypeBuilder<Bage> builder)
        {
            builder.ToTable("Bages");

            builder.HasIndex(a => a.Id)
                .IsUnique();
        }
    }
}
