

namespace ACTO.Data
{
    using ACTO.Data.Models;
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System.Reflection;

    public class ACTODbContext : IdentityDbContext<ACTOUser, IdentityRole, string>
    {

        private readonly IConfiguration configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("Default"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        //this was nice!
        public ACTODbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ACTODbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<ExcursionType> ExcursionTypes { get; set; }
        public DbSet<LanguageExcursion> LanguageExcursions { get; set; }
        public DbSet<Language> LanguageTypes { get; set; }
        public DbSet<Liquidation> Liquidations { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

    }
}
