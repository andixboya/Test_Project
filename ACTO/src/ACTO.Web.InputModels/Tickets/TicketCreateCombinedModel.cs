

namespace ACTO.Web.InputModels.Tickets
{
    using ACTO.Web.ViewModels.Tickets;
    using ACTO.Web.ViewModels.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class TicketCreateCombinedModel
    {
        public TicketCreateInputModel Input { get; set; }
        public List<LanguageViewModel> PossibleLanguages { get; set; }
        public TicketExcursionViewModel ChosenExcursion { get; set; }

        public bool ArePending { get; set; }
    }
}
