using Metallurgist.Models;
using Microsoft.EntityFrameworkCore;

namespace Metallurgist.Interfaces
{
    public abstract class MetalPriceDbContextBase : DbContext
    {
        public DbSet<CopperPrice> CopperPrices { get; set; }
        public DbSet<AluminumPrice> AluminumPrices { get; set; }
        public DbSet<IronPrice> IronPrices { get; set; }
    }
}
