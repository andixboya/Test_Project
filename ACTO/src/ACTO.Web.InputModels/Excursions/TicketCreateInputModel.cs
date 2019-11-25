

namespace ACTO.Web.InputModels.Excursions
{
    using ACTO.Web.ViewModels.Customers;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Tickets;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class TicketCreateInputModel
    {
        public TicketCreateInputModel()
        {
            this.PossibleLanguages = new List<LanguageViewModel>();
        }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public int ExcursionId { get; set; }
        public string Notes { get; set; }
        [Display(Name = "Total tourist count:")]
        public int TouristCount => this.ChildCount + this.AdultCount;
        public int TourLanguageId { get; set; }
        public int Discount { get; set; }

        [Display(Name = "Sum after discount")]
        public decimal SumAfterDiscount { get; set; }
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }


        public List<LanguageViewModel> PossibleLanguages { get; set; }
        public TicketExcursionViewModel ChosenExcursion { get; set; }

        public bool AnyTickets { get; set; }

        //there won`t be any sale field for now..., only when we finalize it!
    }
}
