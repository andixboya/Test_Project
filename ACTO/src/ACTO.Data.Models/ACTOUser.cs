
using Microsoft.AspNetCore.Identity;
namespace ACTO.Data.Models
{
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ACTOUser : IdentityUser
    {

        public ACTOUser()
        {
            this.Liquidations = new List<Liquidation>();
        }
        public string FullName { get; set; }

        public bool IsDeleted { get; set; }

        public List<Liquidation> Liquidations { get; set; }
    }
}
