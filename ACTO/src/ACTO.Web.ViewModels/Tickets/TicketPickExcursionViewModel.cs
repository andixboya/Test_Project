

namespace ACTO.Web.ViewModels.Tickets
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class TicketPickExcursionViewModel
    {
    
        public List<TicketExcursionViewModel> Excursions { get; set; }

        public TicketExcursionViewModel PickedExcursion { get; set; }

    }
}
