using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ACTO.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ACTO.Data;
using System.Security.Claims;

namespace ACTO.Web.Areas.Identity.Pages.Account
{
    //for any users to have access to the action !
    //check for more info razor pages
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
       
        //we`ll be needing only the signInManager
        private readonly SignInManager<ACTOUser> _signInManager;
        private readonly UserManager<ACTOUser> _userManager;
        private readonly ACTODbContext context;
       

        public LoginModel(SignInManager<ACTOUser> signInManager, UserManager<ACTOUser> userManager,ACTODbContext context)
        {
            _signInManager = signInManager;
            this._userManager = userManager;
            this.context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = "/Home/Index";

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ReturnUrl = returnUrl;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = "/Home/Index";

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, isPersistent:false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
              

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        #region is default but i`ll skip it for now
        //public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var user = await _userManager.FindByEmailAsync(Input.Email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //    }

        //    var userId = await _userManager.GetUserIdAsync(user);
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var callbackUrl = Url.Page(
        //        "/Account/ConfirmEmail",
        //        pageHandler: null,
        //        values: new { userId = userId, code = code },
        //        protocol: Request.Scheme);
        //    await _emailSender.SendEmailAsync(
        //        Input.Email,
        //        "Confirm your email",
        //        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //    ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        //    return Page();
        //}
        #endregion
    }
}
