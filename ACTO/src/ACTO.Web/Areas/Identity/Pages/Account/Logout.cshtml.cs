using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ACTO.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static ACTO.Web.Areas.Identity.Pages.Account.LoginModel;

namespace ACTO.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public readonly SignInManager<ACTOUser> _signInManager;
        public readonly UserManager<ACTOUser> _userManager;
        private readonly ILogger<LogoutModel> _logger;

        

        public LogoutModel(SignInManager<ACTOUser> signInManager, UserManager<ACTOUser> userManager, ILogger<LogoutModel> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
             await _signInManager.SignOutAsync();

             _logger.LogInformation("User logged out.");

            return  RedirectToPage("Logout","AsyncGetView");
        }

        
        //TODO: should it be async? 
        public IActionResult OnGetAsyncGetView()
        {
            return this.Page();
        }

    }
}
