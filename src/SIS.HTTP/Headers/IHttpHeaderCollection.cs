//TODO: play with the renaming of the namespaces (dont rely on resharper!)

using System.Collections.Generic;

namespace SIS.HTTP.Headers.Contracts
{
    public interface IHttpHeaderCollection
    {
        Dictionary<string, HttpHeader> HttpHeaders { get; set; }

        void AddHeader(HttpHeader header);

        bool ContainsHeader(string key);

        HttpHeader GetHeader(string key);
    }
}
