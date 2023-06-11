using Metallurgist.Interfaces;
using Metallurgist.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Metallurgist.Data
{
    public class MetalPriceDbContext : MetalPriceDbContextBase
    {
        public DbSet<CopperPrice> CopperPrices { get; set; }
        public DbSet<AluminumPrice> AluminumPrices { get; set; }
        public DbSet<IronPrice> IronPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the base class IMetalPrice
            modelBuilder.Entity<AluminumPrice>()
                .HasKey(x => x.Id); // Define the primary key
            modelBuilder.Entity<CopperPrice>()
                .Property(x => x.Price)
                .IsRequired(); // Make the price property required
            modelBuilder.Entity<IronPrice>()
                .Property(x => x.Timestamp)
                .IsRequired(); // Make the timestamp property required

            // Configure the derived classes AluminumPrice, CopperPrice and IronPrice
            modelBuilder.Entity<AluminumPrice>()
                .ToTable("AluminumPrices"); // Map to a separate table
            modelBuilder.Entity<CopperPrice>()
                .ToTable("CopperPrices"); // Map to a separate table
            modelBuilder.Entity<IronPrice>()
                .ToTable("IronPrices"); // Map to a separate table
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Database = "metallurgist",
                Username = "metallurgist",
                Password = "metallurgist",
                Port = 5432 // default PostgreSQL port
            };

            optionsBuilder.UseNpgsql(connectionStringBuilder.ToString());
        }
    }
}