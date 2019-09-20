using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stopify.Data;
using Stopify.Data.Models;

namespace Stopify.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            //1) default
            services.AddDbContext<StopifyDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //2) default
            //sets up the authentication providers! 
            services.AddIdentity<StopifyUser,IdentityRole>()
                //we`ll be making our own ui!
                
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<StopifyDbContext>()
                .AddDefaultTokenProviders();

            //3) this is custom, for setting the password
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            });

            //4) there is a special option for jwt!

            //5) for services here!


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region unnecessary for now
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            //    app.UseHsts();
            //}
            #endregion

            #region seeding of initial Roles (Admin and User)

            using (var scope = app.ApplicationServices.CreateScope())
            {

                using (var context= scope.ServiceProvider.GetRequiredService<StopifyDbContext>())
                {

                    
                    context.Database.EnsureCreated();

                    var users = context.Users.ToList();

                    if (users.Count==0)
                    {
                        context.Roles.AddRangeAsync(new IdentityRole()
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        } , 
                        new IdentityRole()  
                        {
                            Name="User",
                            NormalizedName="USER"
                        });

                        context.SaveChangesAsync();
                    } 

                }

            }

            #endregion

            app.UseDeveloperExceptionPage();

            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
