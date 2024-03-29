﻿using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Web.InputModels;
using Stopify.Web.ViewModels.Home.Index;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
            services.AddIdentity<StopifyUser, IdentityRole>()
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
            
            //5) for services here!
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IReceiptService, ReceiptService>();


            #region note: cache
            //with the in-memory dictionary object in the controller!
            services.AddMemoryCache(opt =>
            {
                //you can add additional options here! They are not too many!
                //opt. 

            });
            //response caching (with attributes) 
            services.AddResponseCaching(opt =>
            {
                //you can add options here as well, ofcourse
            });

            //note: cache- distributed-SQL-server-caching
            //requires 1 power-sehell command for initialization of the table within the db+
            //+  some options below for db-specification
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheEntries";
            });
            #endregion

            //4) there is a special option for jwt!

            



            //note: cloudinary
            #region cloudinary setup
            Account cloudinaryAccount = new Account
                (
                this.Configuration["Cloudinary:Name"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]
                );

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinaryUtility);
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            //is this necessary? 

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;



            AutoMapperConfig.RegisterMappings(
                typeof(ProductCreateInputModel).GetTypeInfo().Assembly,
                typeof(ProductHomeViewModel).GetTypeInfo().Assembly,
                typeof(ProductServiceModel).GetTypeInfo().Assembly);

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

                using (var context = scope.ServiceProvider.GetRequiredService<StopifyDbContext>())
                {


                    context.Database.EnsureCreated();
                    var users = context.Users.ToList();

                    if (users.Count == 0)
                    {
                        context.Roles.AddRangeAsync(new IdentityRole()
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new IdentityRole()
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        });

                        
                    }

                    if (!context.OrderStatuses.Any())
                    {
                        context.OrderStatuses.AddRangeAsync(new OrderStatus()
                        {
                            Name = "Active"
                        }, new OrderStatus()
                        {
                            Name = "Completed"
                        });

                    }

                    context.SaveChangesAsync();
                }

            }

            #endregion



            app.UseDeveloperExceptionPage();

            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            //note: cache-related
            app.UseResponseCaching();

            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //sample: administration / product / create(примерно!)->как работи

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
