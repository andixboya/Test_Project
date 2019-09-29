using Stopify.Services.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stopify.Web.ViewModels.Order
{
    public class OrderCartViewModel 
    {
        
        public string Id { get; set; }
        
        public string ProductPicture { get; set; }
        
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        
        public decimal ProductPrice { get; set; }


    }
}
