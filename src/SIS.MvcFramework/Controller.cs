using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.MvcFramework.Extensions;
using SIS.MvcFramework.Identity;
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

        protected bool IsLoggedIn()
        {
            //old one
            //return this.Request.Session.ContainsParameter("username");

            //new one
            //return this.User != null;
            //because it throws execption => връща го през речник и за това не може да го намери!
            
            //second  try
            return this.Request.Session.ContainsParameter("principal");

        }

        protected void SignIn(string id, string username, string email)
        {
            this.Request.Session.AddParameter("principal", new Principal()
            {
                Id = id,
                Username = username,
                Email = email
            });

                //instead of each param load, we load one obj
                //this.Request.Session.AddParameter("id", id);
                //this.Request.Session.AddParameter("username", username);
                //this.Request.Session.AddParameter("email", email);
        }

        protected void SignOut()
        {
            //stays the same
            this.Request.Session.ClearParameters();
        }


        public Principal User => this.Request.Session.ContainsParameter("principal")
            ? (Principal)this.Request.Session.GetParameter("principal")
            : null;


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
            return new XmlResult(obj.ToXml());
        }
        protected ActionResult File(byte[] fileContent)
        {
            return new FileResult(fileContent);
        }
        protected ActionResult Json(object obj)
        {
            return new JsonResult(obj.ToJson());
        }
        protected ActionResult NotFound(string message = "")
        {
            return new NotFoundResult(message);
        }

        //note this is public!
        public IHttpRequest Request { get; set; }

    }
}
