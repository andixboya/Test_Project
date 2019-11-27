

namespace ACTO.Web.ViewModels.Tickets
{
    using ACTO.Web.ViewModels.Customers;
    using ACTO.Web.ViewModels.Refund;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class TicketViewModel
    {
        //additional information about excursion reports.
        //TODO: potentially, i can make a sale instead of ticket, but ... i think this will be ok? 
        public TicketViewModel()
        {
            this.Refunds = new List<RefundViewModel>();
        }

        [Display(Name = "Excursion name")]
        public string ExcursionName { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        
        //i`ll skip this, because it can be searched for in the tickets...
        //public CustomerViewModel Customer { get; set; }
        //within the excursion will be the info about it...
        //prices will be taken from the excursion? 


        public int Discount { get; set; }
        [Display(Name = "Tourist Count")]
        public int TouristCount => this.ChildCount + this.AdultCount;
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }
        public decimal PriceAfterDiscount => (PricePerAdult * AdultCount + PricePerChild * ChildCount) * (100.00m - Discount) / 100.00m;
        public List<RefundViewModel> Refunds { get; set; }

    }
}
