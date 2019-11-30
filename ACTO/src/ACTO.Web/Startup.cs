using ACTO.Data;
using ACTO.Data.Models;
using ACTO.Services.Excursion;
using ACTO.Web.Initializers;
using ACTO.Web.InputModels.Excursions;
using ACTO.Web.ViewModels.Sales;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ACTO.Services.Mapping;
using System.Globalization;
using System.Reflection;
using ACTO.Services.Others;
using ACTO.Services.Finance;

namespace ACTO.Web
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
            services.AddDbContext<ACTODbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ACTOUser, IdentityRole>()
                //NOTE: the below are also necessary! for the db initialization!!!
                .AddEntityFrameworkStores<ACTODbContext>()
                .AddDefaultTokenProviders();


            //for seeder 
            services.AddTransient<Initializer>();

            services.AddMvc(opt => opt.EnableEndpointRouting = false);

            #region addition of custom view-routing! (this is not route searching)
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/Liquidation/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/Tickets/{0}" + RazorViewEngine.ViewExtension);
            });
            #endregion


            #region creation of services
            services.AddScoped<ILanguageServices, LanguageServices>();
            services.AddScoped<IExcursionServices, ExcursionServices>();
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddScoped<ISaleServices, SaleServices>();
            services.AddScoped<ILiquidationServices, LiquidationServices>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //for time setting! 
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            #region addition of automapper configs

            //3 classes, from each place ,where the models will be extracted.
            AutoMapperConfig.RegisterMappings(
                typeof(ExcursionCreateInputModel).GetTypeInfo().Assembly,
                typeof(SaleCreateTicketViewModel).GetTypeInfo().Assembly);
            #endregion


            #region by default added
            //if (env.IsDevelopment())
            //    {
            //        app.UseDeveloperExceptionPage();
            //        app.UseDatabaseErrorPage();
            //    }
            //    else
            //    {
            //        app.UseExceptionHandler("/Home/Error");
            //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //        app.UseHsts();
            //    }
            #endregion


            #region initial database seeding/migration
            //new c# 8 syntax i think? 
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ACTODbContext>();

            //context.Database.EnsureDeleted();
            //context.Database.Migrate();

            //adds initial roles ( here i`ll add seller, excursion operator/ cashier)
            var dbInitializer = scope.ServiceProvider.GetRequiredService<Initializer>();
            dbInitializer.SeedRoles();
            #endregion


            //adding custom view search
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "mvcAreaRoute",
                   template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

