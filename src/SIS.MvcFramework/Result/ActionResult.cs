

using System.Net;
using System.Transactions;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Result
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public abstract class ActionResult :HttpResponse
    {
        protected ActionResult(HttpResponseStatusCode httpResponseStatus ) 
        :base(httpResponseStatus)
        {
                
        }
    }
}
