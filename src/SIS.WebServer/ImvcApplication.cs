
namespace SIS.MvcFramework
{
    using SIS.WebServer.Routing;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMvcApplication
    {
        void ConfigureServices();
        void Configure(ServerRoutingTable serverRoutingTable);


    }
}
