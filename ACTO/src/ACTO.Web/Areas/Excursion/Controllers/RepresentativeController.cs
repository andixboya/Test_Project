

namespace ACTO.Web.Areas.Excursion.Controllers
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using ACTO.Web.InputModels.Sale;
    using ACTO.Web.InputModels.Tickets;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Sales;
    using ACTO.Web.ViewModels.Tickets;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    [Area("Excursion")]
    public class RepresentativeController : Controller
    {
        private readonly ACTODbContext context;
        public RepresentativeController(ACTODbContext db)
        {
            this.context = db;
        }

        [HttpGet]
        public async Task<IActionResult> TicketPickExcursion()
        {
            var excursionPickViewModel = new TicketPickExcursionViewModel()
            {
                Excursions = await context.Excursions.Select(e => new TicketExcursionViewModel
                {
                    AvailableSpots = e.AvailableSpots,
                    PricePerChild = e.PricePerChild,
                    Id = e.Id,
                    Name = e.ExcursionType.Name,
                    PricePerAdult = e.PricePerAdult,
                    Departure = e.Departure,
                    Arrival = e.Arrival,
                    StartPoint = e.StartingPoint,
                    EndPoint = e.EndPoint

                })
                .ToListAsync()
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.ViewData["AnyTickets"] = await context.Tickets.Where(t => t.Representative.UserId == userId).AnyAsync(t => t.SaleId == 0);


            return View(excursionPickViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> TicketCreate(TicketPickExcursionViewModel model)
        {
            var excursion = model.PickedExcursion;

            if (excursion is null)
            {
                //TODO: proper exception !
                this.Redirect("/Home/Index");
            }

            var spokenLanguages = await context.LanguageTypes.Where(l => l.LanguageExcursions.Any(le => le.ExcursionId == excursion.Id)).Select(x => new LanguageViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            var viewModel = new TicketCreateCombinedModel()
            {
                //input model will be filled!
                ChosenExcursion = excursion,
                PossibleLanguages = spokenLanguages
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.ViewData["AnyTickets"] = await context.Tickets.Where(t => t.Representative.UserId == userId).AnyAsync(t => t.SaleId == 0);

            
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TicketCreate(TicketCreateCombinedModel model)
        {
            var ticket = model.Input;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (ticket is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = ticket.Customer.FirstName,
                LastName = ticket.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = ticket.AdultCount,
                ChildrenCount = ticket.ChildCount,
                ExcursionId = ticket.ExcursionId,
                Notes = ticket.Notes,
                TourLanguageId = ticket.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId

            };
            await context.Tickets.AddAsync(ticketToAdd);


            await context.SaveChangesAsync();

            return this.Redirect("/Home/Index");
        }


        [HttpPost]
        public async Task<IActionResult> TicketCreateAnother(TicketCreateCombinedModel model)
        {
            var ticket = model.Input;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (ticket is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = ticket.Customer.FirstName,
                LastName = ticket.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = ticket.AdultCount,
                ChildrenCount = ticket.ChildCount,
                ExcursionId = ticket.ExcursionId,
                Notes = ticket.Notes,
                TourLanguageId = ticket.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId

            };
            await context.Tickets.AddAsync(ticketToAdd);

            await context.SaveChangesAsync();

            //it will still buy the tickets, but redirect to the pick Excursion button
            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllPending(TicketCreateCombinedModel model)
        {
            //buy ticket , which is copy from above, and then redirect to TicketPickExcursion

            //it will still buy the tickets, but redirect to the pick Excursion button
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tickets = await context.Tickets.Where(t => t.Representative.UserId == userId).ToListAsync();

            context.Tickets.RemoveRange(tickets);
            await context.SaveChangesAsync();

            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }
        [HttpGet]
        public async Task<IActionResult> TicketCheckOut(TicketCreateCombinedModel model)
        {
            //again create the ticket here, afterwards gather your info about the sale...
            //the below is creation of ticket
            var ticket = model.Input;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (ticket is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = ticket.Customer.FirstName,
                LastName = ticket.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = ticket.AdultCount,
                ChildrenCount = ticket.ChildCount,
                ExcursionId = ticket.ExcursionId,
                Notes = ticket.Notes,
                TourLanguageId = ticket.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId

            };
            await context.Tickets.AddAsync(ticketToAdd);

            await context.SaveChangesAsync();


            //the transition into CheckOut
            var viewModel = new SaleCreateCombinedModel();


            viewModel.TicketsViews= await context.Tickets.Where(t => t.Representative.UserId == userId && t.SaleId == 0).Select(t => new SaleCreateTicketViewModel
            {
                TicketId = t.Id,
                AdultCount =t.AdultCount,
                ChildCount=t.ChildrenCount,
                ExcursionName=t.Excursion.ExcursionType.Name,
                PricePerAdult=t.Excursion.PricePerAdult,
                PricePerChild=t.Excursion.PricePerChild
            })
                .ToListAsync();

            viewModel.TicketsViews.ForEach(v =>
            {
                viewModel.TicketIds.Add(v.TicketId);
            });

            viewModel.RepresentativeId = repId;
            


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TicketCheckOut(SaleCreateCombinedModel model,
            List<SaleCreateTicketViewModel> TicketsViews)
        {
            //here we`ll 
            //gеt cc, cash, and we`ll just add the id to the tickets, so they can be checked
            // count+ prices + discount 
            //1 js, which will calculate the rest

            //we`ll create a sale
            
            //
            ;

            return View();
        }

        //refund ticket 
        

        //refund ticket post
        
        //        {
        //          subtract the count from each ticket and then subtract the sum from the sale
        //         we`ll add checkup if any ticket has count, and that`s it ? 
        //          }

    }
}
