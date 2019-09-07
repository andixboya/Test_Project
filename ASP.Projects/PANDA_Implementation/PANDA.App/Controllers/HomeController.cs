using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PANDA.App.Models;

namespace PANDA.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var x = this.User.Identity.Name;

          
           
            return View();
        }

       
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
