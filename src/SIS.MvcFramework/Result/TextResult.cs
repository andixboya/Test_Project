

namespace SIS.MvcFramework.Result
{
    using System.Text;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class TextResult : ActionResult
    {
        //for texts or sth. else
        public TextResult(string content, HttpResponseStatusCode responseStatusCode, 
            string contentType = "text/plain; charset=utf-8") : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        //for files or pictures
        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode,
            string contentType = "text/plain; charset=utf-8") : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = content;
        }
    }
}
