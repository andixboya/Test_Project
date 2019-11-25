

namespace ACTO.Web.Areas.Excursion.Controllers
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using ACTO.Web.InputModels.Finance;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.InputModels.Excursions;
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

        [HttpPost]
        public async Task<IActionResult> TicketPickExcursion(int id)
        {
            return this.View();
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
                .ToListAsync(),
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


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var input = new TicketCreateInputModel()
            {
                ChosenExcursion = excursion,
                PossibleLanguages = spokenLanguages,
                ExcursionId = excursion.Id,
                AnyTickets = await context.Tickets.Where(t => t.Representative.UserId == userId).AnyAsync(t => t.SaleId == 0)
            };

            return this.View(input);
        }

        //TODO: this will probably be redundant?
        [HttpPost]
        public async Task<IActionResult> TicketCreate(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (model is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = model.AdultCount,
                ChildrenCount = model.ChildCount,
                ExcursionId = model.ExcursionId,
                Notes = model.Notes,
                TourLanguageId = model.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId,
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount

            };
            await context.Tickets.AddAsync(ticketToAdd);


            await context.SaveChangesAsync();

            return this.Redirect("/Excursion/Representative/TicketCheckOut");
        }


        [HttpPost]
        public async Task<IActionResult> TicketCreateAnother(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (model is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = model.AdultCount,
                ChildrenCount = model.ChildCount,
                ExcursionId = model.ExcursionId,
                Notes = model.Notes,
                TourLanguageId = model.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId,
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount
            };
            await context.Tickets.AddAsync(ticketToAdd);


            await context.SaveChangesAsync();

            //it will still buy the tickets, but redirect to the pick Excursion button
            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllPending(TicketCreateInputModel model)
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
        public async Task<IActionResult> TicketCheckOut(TicketCreateInputModel model)
        {
            //again create the ticket here, afterwards gather your info about the sale...
            //the below is creation of ticket
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var representativeId = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = representativeId.Id;
            if (repId == 0)
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (model is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            //i won`t be making any special checkup for customers (not worth it i think?) 
            var customer = new Customer()
            {
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
            };
            await context.Customers.AddAsync(customer);

            var ticketToAdd = new Ticket()
            {
                AdultCount = model.AdultCount,
                ChildrenCount = model.ChildCount,
                ExcursionId = model.ExcursionId,
                Notes = model.Notes,
                TourLanguageId = model.TourLanguageId,
                Customer = customer,
                RepresentativeId = repId,
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount,

            };
            await context.Tickets.AddAsync(ticketToAdd);


            await context.SaveChangesAsync();

            //the transition into CheckOut
            var inputModel = new SaleCreateInputModel();


            inputModel.TicketsViews = await context.Tickets.Where(t => t.Representative.UserId == userId && t.SaleId == 0).Select(t => new SaleCreateTicketViewModel
            {
                TicketId = t.Id,
                AdultCount = t.AdultCount,
                ChildCount = t.ChildrenCount,
                ExcursionName = t.Excursion.ExcursionType.Name,
                PricePerAdult = t.Excursion.PricePerAdult,
                PricePerChild = t.Excursion.PricePerChild,
                Discount = t.Discount
            })
                .ToListAsync();
            inputModel.TicketsViews.ForEach(v =>
            {
                inputModel.TicketIds.Add(v.TicketId);
            });
            inputModel.TotalSumAfterDiscount = inputModel.TicketsViews.Any() ? inputModel.TicketsViews.Sum(t => t.TotalSum) : 0;

            inputModel.RepresentativeId = repId;

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> TicketCheckOut(SaleCreateInputModel model)
        {
            model.IsInvalid = model.TotalAccumulated != model.TotalSumAfterDiscount;
            if (model.IsInvalid)
            {
                model.IsInvalid = true;
                model.TicketsViews = await context.Tickets.Where(t => t.Representative.Id == model.RepresentativeId).Select(t => new SaleCreateTicketViewModel
                {
                    TicketId = t.Id,
                    AdultCount = t.AdultCount,
                    ChildCount = t.ChildrenCount,
                    ExcursionName = t.Excursion.ExcursionType.Name,
                    PricePerAdult = t.Excursion.PricePerAdult,
                    PricePerChild = t.Excursion.PricePerChild,
                    Discount = t.Discount,

                })
              .ToListAsync();
                model.TicketsViews.ForEach(v =>
                {
                    model.TicketIds.Add(v.TicketId);
                });

                return this.View(model);
            }

            var sale = new Sale()
            {
                Cash = model.Cash,
                CreditCard = model.CreditCard,
                RepresentativeId = model.RepresentativeId,
                Tickets = await context.Tickets.Where(t => model.TicketIds.Contains(t.Id))
                .ToListAsync()
                ,
                TotalPrice = model.TotalSumAfterDiscount,
            };

            foreach (var t in sale.Tickets)
            {
                t.Sale = sale;
            }

            await context.Sales.AddAsync(sale);
            //now the question is, if  it will make the connections itself or it won`t ? 
            await context.SaveChangesAsync();


            return View("/Areas/Finance/Views/Sales/ReceiptForm.cshtml");
        }

        //refund ticket 

        public async Task<IActionResult> TicketSearchById()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TicketRefundCreate(int id)
        {
            var ticket = await context.Tickets
                .Include(t => t.Excursion)
                .Include(t => t.Refunds)
                .Select(x => new
                {
                    x.Id,
                    x.AdultCount,
                    x.ChildrenCount,
                    Excursion = new
                    {
                        x.Excursion.PricePerAdult,
                        x.Excursion.PricePerChild
                    },
                    Refunds = x.Refunds.Select(y => new
                    {
                        y.AdultCount,
                        y.ChildCount
                    }).ToList()
                })
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket is null)
            {
                return Redirect("/Excursion/Representative/TicketSearchById");
            }

            var model = new RefundCreateInputModel()
            {
                MaxAdultCount = ticket.AdultCount - ticket.Refunds.Sum(r => r.AdultCount),
                MaxChildrenCount = ticket.ChildrenCount - ticket.Refunds.Sum(r => r.ChildCount),
                PricePerAdult = ticket.Excursion.PricePerAdult,
                PricePerChild = ticket.Excursion.PricePerChild,
                TicketId = ticket.Id,

            };
            model.SumToRefund = model.MaxAdultCount * model.PricePerAdult + model.MaxChildrenCount * model.PricePerChild;
            //view of the ticket ( with count of adult, chidlren  (max out-ed)) and below price to return

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TicketRefundCreate(RefundCreateInputModel model)
        {
            //validate the cases
            if (model.MaxAdultCount < model.AdultToRefund ||
                model.MaxChildrenCount < model.ChildrenToRefund ||
                model.AdultToRefund < 0 ||
                model.ChildrenToRefund < 0)
            {
                var returnVIewModel = await context.Tickets
               .Include(t => t.Excursion)
               .Include(t => t.Refunds)
               .Select(x => new
               {
                   x.Id,
                   x.AdultCount,
                   x.ChildrenCount,
                   Excursion = new
                   {
                       x.Excursion.PricePerAdult,
                       x.Excursion.PricePerChild
                   },
                   Refunds = x.Refunds.Select(y => new
                   {
                       y.AdultCount,
                       y.ChildCount
                   }).ToList()
               })
               .FirstOrDefaultAsync(t => t.Id == model.TicketId);

                if (returnVIewModel is null)
                {
                    return Redirect("/Excursion/Representative/TicketSearchById");
                }

                var input = new RefundCreateInputModel()
                {
                    MaxAdultCount = returnVIewModel.AdultCount - returnVIewModel.Refunds.Sum(r => r.AdultCount),
                    MaxChildrenCount = returnVIewModel.ChildrenCount - returnVIewModel.Refunds.Sum(r => r.ChildCount),
                    PricePerAdult = returnVIewModel.Excursion.PricePerAdult,
                    PricePerChild = returnVIewModel.Excursion.PricePerChild,
                    TicketId = returnVIewModel.Id
                };

                //todo+ add one bool for validation + message!
                return this.View(input);
            }

            var ticket = await context.Tickets.Include(t => t.Sale).FirstOrDefaultAsync(t => t.Id == model.TicketId);

            var cashToRefund = model.AdultToRefund * model.PricePerAdult + model.ChildrenToRefund * model.PricePerChild;

            ticket.Sale.Cash -= cashToRefund;
            ticket.AdultCount -= model.AdultToRefund;
            ticket.ChildrenCount -= model.ChildrenToRefund;
            ticket.Sale.TotalPrice -= cashToRefund;



            var refund = new Refund()
            {
                AdultCount = model.AdultToRefund,
                ChildCount = model.ChildrenToRefund,
                Cash = cashToRefund,
                Date = DateTime.UtcNow,
                TicketId = model.TicketId

            };
            ticket.Refunds.Add(refund);

            context.Update(ticket);

            await context.Refunds.AddAsync(refund);
            await context.SaveChangesAsync();



            return View("/Areas/Excursion/Views/Representative/TicketRefundView.cshtml");
        }




        //TODO:
        //mark for liquidation ( bool == ReadyForLiquidation )
        //1 button, which sets them all for ready 

        //1 get , which lists them all, so he can have a look

        //1 post, which will change their statuses


        //refund ticket post

        //        {
        //          subtract the count from each ticket and then subtract the sum from the sale
        //         we`ll add checkup if any ticket has count, and that`s it ? 
        //          }

    }
}
