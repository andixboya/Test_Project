

using System.Linq;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class UserService :IUserService
    {

        private readonly RunesDbContext context;

        public UserService()
        {
            this.context = new RunesDbContext();
        }

        public User CreateUser(User userForDb)
        {
            userForDb= context.Users.Add(userForDb).Entity;
            context.SaveChanges();
            return userForDb;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return context.Users.FirstOrDefault(user => (user.Username == username
                                                         || user.Email == username)
                                                        && user.Password == password);
        }
    }
}
