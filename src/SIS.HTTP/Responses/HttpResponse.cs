using System.Collections.Generic;
using System.Text;
using SIS.Common;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {

        private IHttpHeaderCollection headers;
        private IHttpCookieCollection cookies;

        public HttpResponse()
        {
            this.headers = new HttpHeaderCollection();
            this.cookies = new HttpCookieCollection();
            this.Content = new byte[0];
        }

        public HttpResponse(HttpResponseStatusCode statusCode) : this()
        {
            statusCode.ThrowIfNull( nameof(statusCode));
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IReadOnlyDictionary<string, HttpHeader> Headers => this.headers.HttpHeaders;

        public IReadOnlyDictionary<string, HttpCookie> Cookies => this.cookies.HttpCookies;

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.headers.AddHeader(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            //here some modifications , because it was on recursive ... streak
            this.cookies.AddCookie(cookie);
        }

        public byte[] GetBytes()
        {
            //here are the headers and cookies
            byte[] httpResponseBytesWithoutBody = Encoding.UTF8.GetBytes(this.ToString());

            //here is the content
            byte[] httpResponseBytesWithBody = new byte[httpResponseBytesWithoutBody.Length + this.Content.Length];

            //here we connect the content + headers+cookies all together.
            for (int i = 0; i < httpResponseBytesWithoutBody.Length; i++)
            {
                httpResponseBytesWithBody[i] = httpResponseBytesWithoutBody[i];
            }

            for (int i = 0; i < httpResponseBytesWithBody.Length - httpResponseBytesWithoutBody.Length; i++)
            {
                httpResponseBytesWithBody[i + httpResponseBytesWithoutBody.Length] = this.Content[i];
            }

            return httpResponseBytesWithBody;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            //THIS... fucking thing.... cost me 1.5 h... :( 
            result
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetStatusLine()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append($"{this.headers}").Append(GlobalConstants.HttpNewLine);

            if (this.cookies.HasCookies())
            {
                //THIS... fucking thing.... cost me 1.5 h... :( 
                result.Append($"{this.cookies}").Append(GlobalConstants.HttpNewLine);
            }

            result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }
    }
}
