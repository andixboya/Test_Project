
using SIS.MvcFramework.Routing;

namespace SIS.MvcFramework
{
    using SIS.MvcFramework.Routing;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMvcApplication
    {
        void ConfigureServices();
        void Configure(ServerRoutingTable serverRoutingTable);


    }
}
