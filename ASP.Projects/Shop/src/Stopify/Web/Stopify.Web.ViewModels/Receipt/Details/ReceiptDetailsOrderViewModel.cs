

namespace Stopify.Web.ViewModels.Receipt.Details
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ReceiptDetailsOrderViewModel
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
