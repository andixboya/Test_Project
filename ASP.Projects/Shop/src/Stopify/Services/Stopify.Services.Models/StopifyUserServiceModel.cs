using Microsoft.AspNetCore.Identity;
using Stopify.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stopify.Services.Models
{
    public class StopifyUserServiceModel : IdentityUser
    {
        public string FullName { get; set; }

        public List<OrderServiceModel> Orders { get; set; }

    }
}
