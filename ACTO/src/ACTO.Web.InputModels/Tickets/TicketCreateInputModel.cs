

namespace ACTO.Web.InputModels.Tickets
{
    using ACTO.Web.ViewModels.Customers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class TicketCreateInputModel
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public int ExcursionId { get; set; }
        public string Notes { get; set; }
        public int TouristCount => this.ChildCount + this.AdultCount;
        public int TourLanguageId { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }


        //there won`t be any sale field for now..., only when we finalize it!
    }
}
