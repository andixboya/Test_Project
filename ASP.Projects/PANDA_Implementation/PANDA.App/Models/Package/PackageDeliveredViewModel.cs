using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PANDA.App.Models.Package
{
    public class PackageDeliveredViewModel
    {

        public PackageDeliveredViewModel()
        {
            this.Packages = new List<SinglePackageDeliveredViewModel>();
        }

        public List<SinglePackageDeliveredViewModel> Packages {get; set;}
    }

    public class SinglePackageDeliveredViewModel
    {
        public string Description { get; set; }

        public double Weight { get; set;  }

        public string ShippingAddress { get; set; }
        
        public string RecipientName { get; set; }
        public string Id { get; set; }

    }
}
