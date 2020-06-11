using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens")
                .HasOne(t => t.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(t => t.UserId);
        }
    }
}
