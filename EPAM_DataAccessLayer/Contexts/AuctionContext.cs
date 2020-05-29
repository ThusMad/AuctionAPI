using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EPAM_DataAccessLayer.Configurations;
using EPAM_DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPAM_DataAccessLayer.Contexts
{
    public sealed class AuctionContext : IdentityDbContext<ApplicationUser>
    {
        public AuctionContext(DbContextOptions<AuctionContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TokenConfiguration());
            modelBuilder.ApplyConfiguration(new MediaConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new AuctionConfiguration());
            modelBuilder.ApplyConfiguration(new BidConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new BageConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new AuctionCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DefaultPaymentMethodConfiguration());
        }

        public override int SaveChanges()
        {
            var entities = (from entry in ChangeTracker.Entries()
                where entry.State == EntityState.Modified || entry.State == EntityState.Added
                select entry.Entity);

            var validationResults = new List<ValidationResult>();
            if (entities.Any(entity => !Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults)))
            {
                throw new Infrastructure.ValidationException(validationResults);
            }

            return base.SaveChanges();
        }
    }

}
