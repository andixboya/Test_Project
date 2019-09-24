using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Stopify.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //note: logger options for log -setting
            .ConfigureLogging(opt=>
            {
                opt.SetMinimumLevel(LogLevel.Critical);
                //you can add also additional loggers!
                //opt.AddDebug
                //note: logger you can add here filters!
                //opt.AddFilter("System", LogLevel.None);
                //opt.AddFilter("Microsoft", LogLevel.None);
            })
                .UseStartup<Startup>();
    }
}
