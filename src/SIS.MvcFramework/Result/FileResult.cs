using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    public class FileResult : ActionResult
    {
        //NOTE: It is interesting how the files are described! => as attachments(similar to inline, but the disposition is attachment!)
        // if we don`t give info about the content-type it will count it as byte[] and will search for the length!
        
        public FileResult(byte[] fileContent, HttpResponseStatusCode httpResponseStatusCode = HttpResponseStatusCode.Ok) : base(httpResponseStatusCode)
        {
            this.AddHeader(new HttpHeader(HttpHeader.ContentLength, fileContent.Length.ToString()));
            this.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, HttpHeaderConstants.AttachmentMime));
            this.Content = fileContent;
        }

    }
}
