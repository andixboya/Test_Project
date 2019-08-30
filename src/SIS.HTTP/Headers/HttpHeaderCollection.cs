using System.Collections.Generic;
using System.Linq;
using SIS.Common;
using SIS.HTTP.Common;
using SIS.HTTP.Headers.Contracts;


namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        public HttpHeaderCollection()
        {
            this.HttpHeaders = new Dictionary<string, HttpHeader>();
        }

        public Dictionary<string, HttpHeader> HttpHeaders { get; set; }

        public void AddHeader(HttpHeader header)
        {
            header.ThrowIfNull( nameof(header));
            this.HttpHeaders.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            key.ThrowIfNullOrEmpty( nameof(key));
            return this.HttpHeaders.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            key.ThrowIfNullOrEmpty( nameof(key));
            return this.HttpHeaders[key];
        }

        public override string ToString() => string.Join("\r\n",
            this.HttpHeaders.Values.Select(header => header.ToString()));

    }
}