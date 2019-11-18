
using Microsoft.AspNetCore.Identity;
namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ACTOUser :IdentityUser
    {
        public string FullName { get; set; }

    }
}
