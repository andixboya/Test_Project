using Microsoft.AspNetCore.Mvc;
using Panda.Domain;
using PANDA.App.Models.User;
using PANDA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PANDA.App.Controllers
{
    public class UserController :Controller
    {

        private readonly PandaDbContextThree context;

        public UserController(PandaDbContextThree context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/User/Login");
            }

            var user = this.context
                .Users
                .FirstOrDefault(u => u.PasswordHash == inputModel.Password && u.UserName == inputModel.Username);

            if (user is null)
            {
                return this.Redirect("/User/Login");
            }
    
            


            return this.Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult Register(UserRegisterInputModel inputModel)
        {

            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/User/Register");
            }

            if (inputModel.Password!= inputModel.ConfirmPassword)
            {
                return this.Redirect("/User/Register");
            }

            this.context.Users.Add(new PandaUser
            {
                UserName = inputModel.Username,
                PasswordHash = inputModel.Password
            });

            this.context.SaveChanges();
            return this.Redirect("/User/Login");
        }

    }
}
