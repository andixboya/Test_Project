

namespace IRunes.App
{
    using SIS.MvcFramework;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Program
    {
        public static void Main( )
        {
            WebHost.Start(new StartUp());
        }
    }
}
