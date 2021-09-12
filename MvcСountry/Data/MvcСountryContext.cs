using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcCountry.Models;
using MvcСountry.Models;

namespace MvcСountry.Data
{
    public class MvcСountryContext : DbContext
    {
        public MvcСountryContext (DbContextOptions<MvcСountryContext> options)
            : base(options)
        {
        }
        
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
            .HasOne(b => b.Country)
            .WithOne(i => i.City)
            .HasForeignKey<Country>(b => b.CityForeignKey);

            modelBuilder.Entity<Regions>()
             .HasOne(b => b.Country)
             .WithOne(i => i.Regions)
             .HasForeignKey<Country>(b => b.RegionForeignKey);
        }
    }
}
