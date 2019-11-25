

namespace ACTO.Web.InputModels.Finance
{
    using ACTO.Web.ViewModels.Sales;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    public class SaleCreateInputModel
    {
        public SaleCreateInputModel()
        {
            this.TicketsViews = new List<SaleCreateTicketViewModel>();
            this.TicketIds = new List<int>();
        }
        public List<SaleCreateTicketViewModel> TicketsViews { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public int RepresentativeId { get; set; }
        public List<int> TicketIds { get; set; }

        [Display(Name = "Total Sum:")]
        public decimal TotalSumAfterDiscount { get; set; }

        public decimal TotalAccumulated { get; set; }

        public bool IsInvalid { get; set; }
    }
}
