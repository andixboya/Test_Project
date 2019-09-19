

namespace Stopify.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StopifyUser  : IdentityUser
    {
        public StopifyUser()
        {

        }


        public string FullName { get; set; }

    }
}
