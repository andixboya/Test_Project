using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Stopify.Data.Models;

namespace Stopify.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<StopifyUser> _signInManager;
        

        public LogoutModel(SignInManager<StopifyUser> signInManager)
        {
            _signInManager = signInManager;
            
        }

        public  async Task<IActionResult> OnGet(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {

                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }

        }

        //public async Task<IActionResult> OnPost(string returnUrl = null)
        //{
        //    return null;
        //}
    }
}