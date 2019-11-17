

namespace ACTO.Data
{
    using ACTO.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ACTODbContext : IdentityDbContext<ACTOUser, IdentityRole, string>
    {

        private readonly IConfiguration configuration;

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


    }
}
