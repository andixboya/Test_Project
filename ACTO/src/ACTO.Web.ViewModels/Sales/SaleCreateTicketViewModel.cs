
namespace ACTO.Web.ViewModels.Sales
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SaleCreateTicketViewModel
    {
        
        public int TicketId { get; set; }
        public string ExcursionName { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public decimal PricePerAdult { get; set; }

        public decimal PricePerChild { get; set; }
        public int Discount { get; set; }

        [Display(Name = "Total:")]
        public decimal TotalSum => (AdultCount * PricePerAdult + ChildCount * PricePerChild) * (100.00m-Discount)/100.00m;



    }
}
