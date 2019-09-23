
namespace Stopify.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public DateTime ManufacturedOn { get; set; }
    }
}
