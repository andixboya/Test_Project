

namespace PANDA.App.Models.Package
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class PackageCreateInputModel
    {

        public PackageCreateInputModel()
        {
            this.Users = new List<string>();
        }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public string Recipient { get; set; }

        public List<string> Users { get; set; }

    }
}
