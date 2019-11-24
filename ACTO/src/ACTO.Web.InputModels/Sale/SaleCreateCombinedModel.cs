

namespace ACTO.Web.InputModels.Sale
{
    using ACTO.Web.ViewModels.Sales;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class SaleCreateCombinedModel
    {
        public SaleCreateCombinedModel()
        {
            this.TicketIds = new List<int>();
        }

        public List<SaleCreateTicketViewModel> TicketsViews { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public int RepresentativeId { get; set; }
        public List<int> TicketIds { get; set; }
        public int Discount { get; set; }

    }
}
