
using SIS.MvcFramework.Routing;

namespace IRunes.App
{
    using IRunes.App.Controllers;
    using IRunes.Data;
    using SIS.HTTP.Enums;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Routing;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StartUp : IMvcApplication
    {
        //routing is for every app. the same? ( that is why it is 
        public void Configure(ServerRoutingTable serverRoutingTable)
        {
            //for db
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }

        }

        

        public void ConfigureServices()
        {
            
        }
    }
}
