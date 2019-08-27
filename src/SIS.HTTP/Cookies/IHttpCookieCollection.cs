using System.Collections.Generic;

//TODO: play with the renaming of the namespaces!
namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookieCollection : IEnumerable<HttpCookie>
    {

        Dictionary<string, HttpCookie> HttpCookies { get; set; }

        void AddCookie(HttpCookie httpCookie);

        bool ContainsCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
