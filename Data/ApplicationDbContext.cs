using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Models;

namespace PROG6212_CMCS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> MyUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Claim>().ToTable("Claims");
            builder.Entity<Document>().ToTable("Document");
            builder.Entity<Notification>().ToTable("Notification");
            builder.Entity<Payment>().ToTable("Payment");
            builder.Entity<User>().ToTable("User");

            builder.Entity<Claim>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Document>()
                .HasOne(d => d.Claim)
                .WithMany()
                .HasForeignKey(d => d.ClaimID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}