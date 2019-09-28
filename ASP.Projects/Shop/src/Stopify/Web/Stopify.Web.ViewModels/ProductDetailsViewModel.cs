

namespace Stopify.Web.ViewModels
{
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ProductDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }


        public string ProductType { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public DateTime ManufacturedOn { get; set; }
    }
}
