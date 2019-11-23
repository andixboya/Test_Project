using ACTO.Data;
using ACTO.Data.Models;
using ACTO.Data.Models.Excursions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACTO.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        private readonly Microsoft.AspNetCore.Identity.UserManager<ACTOUser> _userManager;
        private readonly ACTODbContext context;
        //private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(Microsoft.AspNetCore.Identity.UserManager<ACTOUser> userManager, ILogger<RegisterModel> logger, ACTODbContext context)
        {
            this._userManager = userManager;
            this.context = context;
            //_logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Display(Name = "Role")]
            public string Role { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public SelectList Options { get; set; }

        }

        public async Task OnGetAsync()
        {

            this.Input = new InputModel
            {
                Options = new SelectList(await this.context.Roles.Where(r => r.Name != "Admin").ToListAsync())
            };

        }

        //here it will be binded automatically (the input model i think)
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = "/Identity/Account/Login";

            //how to get the specific types of user via role 
            //var users = await _userManager.GetUsersInRoleAsync("User");

            if (ModelState.IsValid)
            {
                //unnecessary for now 
                //var isAdmin = !this._userManager.Users.Any();
                var user = new ACTOUser { UserName = this.Input.Username, Email = this.Input.Email };

                //creates and hashses the password
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //assignes the selected role
                    await this._userManager.AddToRoleAsync(user, this.Input.Role);
                    await this.DistributeToAppropriateRole(user);
                    //we won`t be needing the logger for now... 
                    //_logger.LogInformation("User created a new account with password.");

                    return LocalRedirect(returnUrl);
                }


                //for error checkups 
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            //returns to the page with the loaded erors from the above foreach.
            return Page();
        }

        private async Task DistributeToAppropriateRole(ACTOUser user)
        {
            //i can do this with... reflection i think, but not now
            if (this.Input.Role == "Representative")
            {
                var representativeToAdd = new Representative()
                {
                    UserId = user.Id
                };
                await context.Representatives.AddAsync(representativeToAdd);
                
                await context.SaveChangesAsync();
            }


        }

    }

}
