

namespace Stopify.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Stopify.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class StopifyDbContext : IdentityDbContext<StopifyUser, IdentityRole, string>
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }


        public StopifyDbContext()
        {

        }

        public StopifyDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
