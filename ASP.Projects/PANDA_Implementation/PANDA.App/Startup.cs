using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panda.Domain;
using PANDA.Data;

namespace PANDA.App
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //1. first this
        public void ConfigureServices(IServiceCollection services)
        {
            //adding dbContext
            services
                .AddDbContext<PandaDbContextThree>(options=> options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            //adding Identity of User + types of roles
            services.AddIdentity<PandaUser, PandaUserRole>()
                .AddEntityFrameworkStores<PandaDbContextThree>()
                .AddDefaultTokenProviders();


            //configuring options of Identity ( password in particular)
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
            });
            


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        //2. then this. (after configuring the services of the container)
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<PandaDbContextThree>())
                {
                    context.Database.EnsureCreated();

                    if (!context.Roles.Any())
                    {
                        context.Roles.Add(new PandaUserRole { Name = "Admin", NormalizedName = "ADMIN" });
                        context.Roles.Add(new PandaUserRole { Name = "User", NormalizedName = "USER" });
                    }

                    if (!context.PackageStatuses.Any())
                    {
                        context.PackageStatuses.Add(new PackageStatus { Name = "Pending" });
                        context.PackageStatuses.Add(new PackageStatus { Name = "Shipped" });
                        context.PackageStatuses.Add(new PackageStatus { Name = "Delivered" });
                        context.PackageStatuses.Add(new PackageStatus { Name = "Acquired" });
                    }

                    context.SaveChanges();
                }
            }


            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            //this was added
            app.UseDeveloperExceptionPage();


            //and these two, but it works with useMvc as well!
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
