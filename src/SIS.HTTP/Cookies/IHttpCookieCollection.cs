using System.Collections.Generic;

//TODO: play with the renaming of the namespaces!
namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookieCollection : IEnumerable<HttpCookie>
    {
        void AddCookie(HttpCookie httpCookie);

        bool ContainsCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
