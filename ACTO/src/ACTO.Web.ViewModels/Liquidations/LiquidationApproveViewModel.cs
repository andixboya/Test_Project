

namespace ACTO.Web.ViewModels.Liquidations
{
    using ACTO.Web.ViewModels.Tickets;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    public class LiquidationApproveViewModel
    {
        public int LiquidationId { get; set; }
        public List<TicketViewModel> Tickets { get; set; }

        [Display(Name = "Total sum due:")]
        public decimal TotalOwned { get; set; }

        [Display(Name = "Cash:")]
        public decimal Cash { get; set; }

        [Display(Name = "Credit card:")]
        public decimal CreditCard { get; set; }

        [Display(Name = "Total accumulated:")]
        public decimal TotalAccumulated => this.Cash + this.CreditCard;
        public bool IsInvalid { get; set; }
    }
}
