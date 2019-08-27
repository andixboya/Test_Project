using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.MvcFramework.Result;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData;

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in this.ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", 
                    param.Value.ToString());
            }

            return viewContent;
        }

        protected bool IsLoggedIn(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected void SignIn(IHttpRequest httpRequest, string id, string username, string email)
        {
            httpRequest.Session.AddParameter("id", id);
            httpRequest.Session.AddParameter("username", username);
            httpRequest.Session.AddParameter("email", email);
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        protected ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected ActionResult Xml(object obj)
        {
            return null;
        }
        protected ActionResult File(string path)
        {
            return null;
        }
        protected ActionResult Json(object obj)
        {
            return null;
        }



    }
}
