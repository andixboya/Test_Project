using System;
using System.Collections.Generic;
using System.Text;

namespace Stopify.Web.ViewModels.Receipt.Profile
{
    public class ReceiptProfileViewModel 
    {

        public string Id { get; set; } 

        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }

        public int Products { get; set; }

    }
}
