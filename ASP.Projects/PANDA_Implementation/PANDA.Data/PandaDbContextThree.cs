

namespace PANDA.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Panda.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public  class PandaDbContextThree :IdentityDbContext<PandaUser,PandaUserRole,string>
    {
        public DbSet<PackageStatus> PackageStatuses { get; set; }

        public DbSet<Package> Packages { get; set; }

        //redundant? 
        //public DbSet<PandaUser> PandaUsers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        public PandaDbContextThree(DbContextOptions<PandaDbContextThree> context):
            base(context)
        {

        }
        public PandaDbContextThree()
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<PandaUser>()
                .HasKey(user => user.Id);

            builder.Entity<PandaUser>()
                .HasMany(user => user.Packages)
                .WithOne(package => package.Recipient)
                .HasForeignKey(package => package.RecipientId);

            builder.Entity<PandaUser>()
                .HasMany(user => user.Receipts)
                .WithOne(receipt => receipt.Recipient)
                .HasForeignKey(receipt => receipt.RecipientId);

            builder.Entity<Receipt>()
                .HasOne(receipt => receipt.Package)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PandaDB;Trusted_Connection=true;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        
    }
}
