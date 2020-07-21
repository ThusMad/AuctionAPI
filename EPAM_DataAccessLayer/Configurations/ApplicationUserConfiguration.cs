using EPAM_DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM_DataAccessLayer.Configurations
{
    class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasOne(bc => bc.Balance)
                .WithOne(b => b.User)
                .HasForeignKey<Balance>(a => a.UserId)
                .IsRequired();

            builder.HasOne(u => u.ProfilePicture);
        }
    }
}
