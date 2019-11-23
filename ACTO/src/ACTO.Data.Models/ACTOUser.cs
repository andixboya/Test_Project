
using Microsoft.AspNetCore.Identity;
namespace ACTO.Data.Models
{
    using ACTO.Data.Models.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ACTOUser :IdentityUser
    {
        public string FullName { get; set; }

        //this should be redundant? 
        public Representative Representative { get; set; }

        public bool IsDeleted { get; set; }

    }
}
