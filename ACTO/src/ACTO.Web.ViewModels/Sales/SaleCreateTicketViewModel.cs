
namespace ACTO.Web.ViewModels.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaleCreateTicketViewModel
    {
        public int TicketId { get; set; }
        public string ExcursionName { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public decimal PricePerAdult { get; set; }

        public decimal PricePerChild { get; set; }


    }
}
