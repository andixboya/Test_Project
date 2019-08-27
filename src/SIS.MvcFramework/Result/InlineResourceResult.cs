using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    public class InlineResourceResult : ActionResult
    {
        //for inline ? (any file, because it is bytes)
        //// if we don`t give info about the content-type it will count it as byte[] and will search for the length!
        public InlineResourceResult(byte[] content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, HttpHeaderConstants.InlineMime));
            this.Content = content;
        }
    }
}
