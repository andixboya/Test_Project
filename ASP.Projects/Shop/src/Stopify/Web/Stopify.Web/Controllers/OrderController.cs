

namespace Stopify.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderController : Controller
    {
        public IActionResult Order (string id, int quantity)
        {

            ;

            return this.Redirect("/Home/Index");
        }
    }
}
