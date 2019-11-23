

namespace ACTO.Web.ViewModels.Tickets
{
    using ACTO.Web.InputModels.Tickets;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class TicketCreateCombinedModel
    {
        public  List<TicketPickExcursionViewModel> Excursions { get; set; }

        public TicketCreateInputModel Input { get; set; }
    }
}
