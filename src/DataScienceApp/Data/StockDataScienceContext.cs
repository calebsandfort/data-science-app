using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataScienceApp.Models;

namespace DataScienceApp.Data
{
    public class DataScienceAppContext : DbContext
    {
        public DataScienceAppContext(DbContextOptions<DataScienceAppContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<PriceEntry> PriceEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceEntry>()
                    .HasOne(g => g.Stock)
                    .WithMany(t => t.Prices)
                    .HasForeignKey(g => g.StockID)
                    .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
        }
    }
}
