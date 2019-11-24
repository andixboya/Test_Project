using ACTO.Web.ViewModels.Excursions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Web.ViewModels.Tickets
{
    public class TicketCreateViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }
        public TicketPickExcursionViewModel Excursion { get; set; }
    }
}
