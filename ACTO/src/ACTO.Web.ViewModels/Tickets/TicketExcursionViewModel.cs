using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACTO.Web.ViewModels.Tickets
{
    public class TicketExcursionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableSpots { get; set; }
        public decimal PricePerChild { get; set; }
        public decimal PricePerAdult { get; set; }

        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }




    }
}
