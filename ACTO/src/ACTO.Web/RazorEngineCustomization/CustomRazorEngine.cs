

namespace ACTO.Web.RazorEngineCustomization
{
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    public class CustomRazorEngine : RazorViewEngine
    {


        public CustomRazorEngine(IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, ILoggerFactory loggerFactory, DiagnosticListener diagnosticListener)
            : base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, loggerFactory, diagnosticListener)
        {

          
        }



    }
}
