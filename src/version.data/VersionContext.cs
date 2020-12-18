using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t3winc.version.domain;

namespace t3winc.version.data
{
    public class VersionContext : DbContext
    {
        public VersionContext(DbContextOptions<VersionContext> options, Serilog.ILogger logger) : base(options)
        {            
            Log.Logger = logger;
        }

        public DbSet<MyVersion> Version { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Branch> Branch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Log.Information("Version:OnModelCreating:Starting");
            modelBuilder.Entity<MyVersion>()
                .HasMany(p => p.Products)
                .WithOne(v => v.Version);

            modelBuilder.Entity<Product>()
                .HasMany(b => b.Branches)
                .WithOne(p => p.Product);

            modelBuilder.Entity<MyVersion>().HasData(
                new {Id = 1, Key = "6b42bb5b-45ae-44ba-b3ef-53b9ef342cfa", Organization = "The Web We Weave, Inc."}
                );

            modelBuilder.Entity<Product>().HasData(
                new {Id = 1, Name="AffirmStore", Master="", Major=1, Minor=1, Patch=0, Revision=214, VersionId = 1 }
                );

            modelBuilder.Entity<Branch>().HasData(
                new {Id = 1, Name="feature/convertChargeToSubscription", Status="Active", Major=1, Minor=2, Patch=0, Revision=237, Version="1.2.0-alpha.237", ProductId=1}
                );

            Log.Information("Version:OnModelCreating:Completed");
        }
    }
}
