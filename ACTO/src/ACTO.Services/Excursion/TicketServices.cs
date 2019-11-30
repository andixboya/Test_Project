

namespace ACTO.Services.Excursion
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using ACTO.Services.Finance;
    using ACTO.Services.Others;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Tickets;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class TicketServices : ITicketServices
    {
        private ACTODbContext context;
        private ICustomerServices customerServices;
        private ISaleServices saleServices;
        private IExcursionServices excursionServices;

        public TicketServices(ACTODbContext context, ICustomerServices customerServices, ISaleServices saleServices, IExcursionServices excursionServices)
        {
            this.context = context;
            this.customerServices = customerServices;
            this.saleServices = saleServices;
            this.excursionServices = excursionServices;

        }

        public async Task<bool> CreateTicket(TicketCreateInputModel model, string userId)
        {
            Customer customer = await this.customerServices.CustomerCreate(model.Customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = model.AdultCount,
                ChildrenCount = model.ChildCount,
                ExcursionId = model.ExcursionId,
                Notes = model.Notes,
                TourLanguageId = model.TourLanguageId,
                Customer = customer,
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount
            };

            var sale = await this.saleServices.GetOrCreateSaleById(userId);

            sale.Tickets.Add(ticketToAdd);
            ticketToAdd.Sale = sale;

            await context.Tickets.AddAsync(ticketToAdd);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<TicketCreateInputModel> FillGetTicketCreateInputModel(TicketExcursionViewModel excursion, List<LanguageViewModel> spokenLanguages, string userId)
        {
            return new TicketCreateInputModel()
            {
                ChosenExcursion = excursion,
                PossibleLanguages = spokenLanguages,
                ExcursionId = excursion.Id,
                AnyTickets = await this.HasPendingTickets(userId)
            };
        }

        public async Task<List<LanguageViewModel>> GetLanguagesForExcursion(int excursionId)
        {
            return await this.excursionServices.GetLanguages(excursionId);
        }

        public async Task<TicketPickExcursionViewModel> GetPossibleExcursionsForTicket()
        {
            return await this.excursionServices.GetExcursionsForTicketCreateView();
        }

        public async Task<bool> HasPendingTickets(string userId)
        {
            return await context
                .Sales
                .AnyAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == userId);
        }
    }
}
