

using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class XmlResult :ActionResult
    {
        //we`ll receive it as text still. (not byte array)
        public XmlResult(string content, HttpResponseStatusCode httpResponseStatus) 
            : base(httpResponseStatus)
        {
            this.AddHeader(new HttpHeader(HttpHeader.ContentType,HttpHeaderConstants.XmlMime));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
