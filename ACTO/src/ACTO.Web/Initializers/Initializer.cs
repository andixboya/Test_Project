using ACTO.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACTO.Web.Initializers
{
    public class Initializer
    {
        private readonly ACTODbContext context;

        public Initializer(ACTODbContext context)
        {
            this.context = context;
        }


        public  void SeedRoles()
        {
            context.Database.EnsureCreated();
            
            if (context.Users.Count() == 0 && context.Roles.Count()==0)
            {
                context.Roles.AddRangeAsync(new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Name = "ExcursionOperator",
                    NormalizedName = "EXCURSIONOPERATOR"
                },
                new IdentityRole()
                {
                    Name="Representative",
                    NormalizedName="REPRESENTATIVE"
                },
                new IdentityRole()
                {
                    Name="Cashier",
                    NormalizedName="CASHIER"
                }
                );
                context.SaveChanges();

                //TODO: add other roles too!
            }

            
            
        }

    }
}
