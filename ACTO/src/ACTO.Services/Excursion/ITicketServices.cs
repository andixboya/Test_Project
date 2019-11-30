

namespace ACTO.Services.Excursion
{
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Tickets;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITicketServices
    {

        Task<bool> HasPendingTickets(string userId);

        Task<TicketCreateInputModel> FillGetTicketCreateInputModel(TicketExcursionViewModel excursion, List<LanguageViewModel> spokenLanguages, string userId);

        Task<bool> CreateTicket(TicketCreateInputModel model, string userId);

        Task<List<LanguageViewModel>> GetLanguagesForExcursion(int excursionId);

        Task<TicketPickExcursionViewModel> GetPossibleExcursionsForTicket();

    }
}
