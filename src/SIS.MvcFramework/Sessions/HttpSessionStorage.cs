using System.Collections.Concurrent;
using SIS.HTTP.Sessions;

namespace SIS.MvcFramework.Sessions
{
    public class HttpSessionStorage : IHttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private readonly ConcurrentDictionary<string, IHttpSession> httpSessions;

        public HttpSessionStorage()
        {
            this.httpSessions = new ConcurrentDictionary<string, IHttpSession>();
        }


        public IHttpSession GetSession(string sessionId)
        {
            return this.httpSessions.GetOrAdd(sessionId, _ => new HttpSession(sessionId));
        }

        public bool ContainsSession(string sessionId)
        {
            return httpSessions.ContainsKey(sessionId);
        }
    }
}
