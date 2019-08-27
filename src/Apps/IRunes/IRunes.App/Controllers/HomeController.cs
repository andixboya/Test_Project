﻿using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        [Authorize]
        public ActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                //here it was bad.
                this.ViewData["Username"] = this.User.Username;
                 
                return this.View("Home");
            }

            return this.View();
        }
    }
}
