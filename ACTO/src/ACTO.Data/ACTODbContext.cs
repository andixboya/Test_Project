

namespace ACTO.Data
{
    using ACTO.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public class ACTODbContext : IdentityDbContext<ACTOUser, IdentityRole, string>
    {

        private readonly IConfiguration configuration;

        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<ExcursionType> ExcursionTypes { get; set; }
        public DbSet<LanguageExcursion> LanguageExcursions { get; set; }
        public DbSet<LanguageType> LanguageTypes { get; set; }

        //this was nice...! :D 
        public ACTODbContext(IConfiguration configuration)
        {
            this.configuration = configuration;

        }

        public ACTODbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

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


    }
}
