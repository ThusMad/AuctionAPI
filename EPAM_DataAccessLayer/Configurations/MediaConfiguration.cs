using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Media")
                .HasOne(b => b.Auction)
                .WithMany(a => a.Images)
                .HasForeignKey(b => b.AuctionId);
        }
    }
}
