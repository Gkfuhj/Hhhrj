using Microsoft.EntityFrameworkCore;
using HawalatySystem.Models;

namespace HawalatySystem.Data
{
    public class HawalatyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<AgentBalance> AgentBalances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=hawalaty.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Transfer configurations
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasIndex(e => e.TransferId).IsUnique();
                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.CreatedTransfers)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CompletedByUser)
                    .WithMany(p => p.CompletedTransfers)
                    .HasForeignKey(d => d.CompletedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.FromAgent)
                    .WithMany(p => p.FromTransfers)
                    .HasForeignKey(d => d.FromAgentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.ToAgent)
                    .WithMany(p => p.ToTransfers)
                    .HasForeignKey(d => d.ToAgentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ExchangeRate configurations
            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasIndex(e => new { e.FromCurrency, e.ToCurrency, e.Date }).IsUnique();
                entity.HasOne(d => d.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserPermission configurations
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.Permission }).IsUnique();
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // AgentBalance configurations
            modelBuilder.Entity<AgentBalance>(entity =>
            {
                entity.HasIndex(e => new { e.AgentId, e.Currency }).IsUnique();
                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.AgentBalances)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    FullName = "System Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    Country = "Libya",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed default exchange rates
            var today = DateTime.Now.Date;
            modelBuilder.Entity<ExchangeRate>().HasData(
                new ExchangeRate
                {
                    Id = 1,
                    FromCurrency = "TRY",
                    ToCurrency = "LYD",
                    Rate = 0.16m,
                    BuyRate = 0.15m,
                    SellRate = 0.17m,
                    Date = today,
                    CreatedByUserId = 1,
                    CreatedDate = DateTime.Now
                },
                new ExchangeRate
                {
                    Id = 2,
                    FromCurrency = "USD",
                    ToCurrency = "LYD",
                    Rate = 4.85m,
                    BuyRate = 4.80m,
                    SellRate = 4.90m,
                    Date = today,
                    CreatedByUserId = 1,
                    CreatedDate = DateTime.Now
                },
                new ExchangeRate
                {
                    Id = 3,
                    FromCurrency = "USD",
                    ToCurrency = "TRY",
                    Rate = 30.50m,
                    BuyRate = 30.30m,
                    SellRate = 30.70m,
                    Date = today,
                    CreatedByUserId = 1,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}