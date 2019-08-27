//TODO: play with the renaming of the namespaces (dont rely on resharper!)
namespace SIS.HTTP.Headers.Contracts
{
    public interface IHttpHeaderCollection
    {
        void AddHeader(HttpHeader header);

        bool ContainsHeader(string key);

        HttpHeader GetHeader(string key);
    }
}
