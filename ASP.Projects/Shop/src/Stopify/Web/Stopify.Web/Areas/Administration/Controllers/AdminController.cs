

namespace Stopify.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    //note:  this is for lock-down of each method!
    [Authorize(Roles = "Admin")]

    //this is for later perhaps
    //note: if  you do not add this, it will not search in area administration! 
    [Area("Administration")]

    public class AdminController :Controller
    {


    }
}
