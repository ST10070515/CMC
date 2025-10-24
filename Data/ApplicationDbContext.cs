using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Models;

namespace PROG6212_CMCS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        // Initialisign the base class (IdentityDbContext)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // === DbSets for all tables in your database ===
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> MyUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 1. Include Identity configuration first
            base.OnModelCreating(builder);

            // === Table Names (Enforce singular names as per SQL schema) ===
            // Note: The Identity tables are implicitly configured (e.g., AspNetUsers, AspNetRoles)
            builder.Entity<Claim>().ToTable("Claims");
            builder.Entity<Document>().ToTable("Document");
            builder.Entity<Notification>().ToTable("Notification");
            builder.Entity<Payment>().ToTable("Payment");
            builder.Entity<User>().ToTable("User");


            // === Relationships ===

            // 1. Claim -> Users (Lecturer, Coordinator, Manager)

            builder.Entity<Claim>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Claim -> Document (One-to-Many)
            builder.Entity<Document>()
                .HasOne(d => d.Claim)
                .WithMany()
                .HasForeignKey(d => d.ClaimID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}