

namespace ACTO.Web.ViewModels.Liquidations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LiquidationPickViewModel
    {
        public int Id { get; set; }

        public string RepName { get; set; }
        public decimal Cash { get; set; }

        public decimal CreditCard { get; set; }

        public decimal Total => this.Cash + this.CreditCard;
    }
}
