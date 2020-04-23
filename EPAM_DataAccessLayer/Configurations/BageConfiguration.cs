using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class BageConfiguration : IEntityTypeConfiguration<Bage>
    {
        public void Configure(EntityTypeBuilder<Bage> builder)
        {
            builder.ToTable("Bages");
        }
    }
}
