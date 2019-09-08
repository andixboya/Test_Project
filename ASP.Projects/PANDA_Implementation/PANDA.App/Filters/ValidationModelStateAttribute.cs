

namespace PANDA.App.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PANDA.App.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ValidateModelStateAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //nothing for now, perhaps it would be useful for... adding some info or sth. 


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //only question is... can the defaultRout be... set, to each controller? as an exit condition?
            if (!context.ModelState.IsValid)
            {
                context.Result = ((Controller)context.Controller).Redirect("/Home/Index");
            }

        }
    }
}
