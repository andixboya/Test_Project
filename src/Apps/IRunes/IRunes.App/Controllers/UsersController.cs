using IRunes.Data;
using IRunes.Models;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
//Note: after the refactoring requests and responses should fall completely! (because request is in the controller, and response is transforrmed 
//into action result
//using SIS.HTTP.Requests;
//using SIS.HTTP.Responses;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        //note: here add non-action, because it does not have to be a route!
        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult LoginConfirm()
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();

                User userFromDb = context.Users.FirstOrDefault(user => (user.Username == username
                                                                        || user.Email == username)
                                                                       && user.Password == this.HashPassword(password));
                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                //this is refactored!
                this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult RegisterConfirm()
        {
            using (var context = new RunesDbContext())
            {
                //all of the requests!
                string username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)this.Request.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)this.Request.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                User user = new User
                {
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/Users/Login");
        }

        public ActionResult Logout()
        {
            //this is refactored within the base!
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
