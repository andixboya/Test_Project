
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class JsonResult : ActionResult
    {
        public JsonResult(string jsonText ,HttpResponseStatusCode httpResponseStatus= HttpResponseStatusCode.Ok) 
            : base(httpResponseStatus)
        {
            this.AddHeader(new HttpHeader(HttpHeader.ContentType, HttpHeaderConstants.JsonMime));
            this.Content = Encoding.UTF8.GetBytes(jsonText);
        }
    }
}
