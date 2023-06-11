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
        public DbSet<LatestMetalPrice> LatestMetalPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LatestMetalPrice>().HasNoKey();
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