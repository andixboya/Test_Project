

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

        [HttpPost]//works!
        public IActionResult TicketPickExcursion(int id)
        {
            return this.View();
        }


        [HttpGet] //works
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
            var rep = await context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);

            this.ViewData["AnyTickets"] = await context
                .Sales
                .AnyAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);


            return View(excursionPickViewModel);
        }

        //this is working
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
            var rep = await context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);

            var input = new TicketCreateInputModel()
            {
                ChosenExcursion = excursion,
                PossibleLanguages = spokenLanguages,
                ExcursionId = excursion.Id,
                AnyTickets = await context.
                Sales.
                AnyAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id)
            };

            return this.View(input);
        }

        [HttpPost] //works too (decoupled it from ticket checkout) 
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
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount
            };


            var sale = await context.
                Sales
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.IsFinalized == false && s.Liquidation.RepresentativeId == repId);

            //if no such sale is available we have to initialize it and  both liquidation if its not there!
            if (sale is null)
            {
                sale = new Sale();

                var liquidation = await context
                    .Liquidations
                    .FirstOrDefaultAsync(l => l.ReadyByRepresentative == false
                    && l.RepresentativeId == repId);


                if (liquidation is null)
                {
                    liquidation = new Liquidation()
                    {
                        RepresentativeId = repId
                    };

                    await context.Liquidations.AddAsync(liquidation);

                }

                liquidation.Sales.Add(sale);
                sale.Liquidation = liquidation;

                await context.Sales.AddAsync(sale);
            }

            sale.Tickets.Add(ticketToAdd);
            ticketToAdd.Sale = sale;

            await context.Tickets.AddAsync(ticketToAdd);
            await context.SaveChangesAsync();

            return this.Redirect("/Excursion/Representative/TicketCheckOut");
        }

        [HttpPost] //works!
        public async Task<IActionResult> TicketCreateAnother(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            var repId = rep.Id;
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
                Discount = model.Discount,
                PriceAfterDiscount = model.SumAfterDiscount
            };


            var sale = await context.Sales.Include(s => s.Tickets).FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == repId);

            //if no such sale is available we have to initialize it and  both liquidation if its not there!
            if (sale is null)
            {
                sale = new Sale();

                var liquidation = await context.Liquidations.FirstOrDefaultAsync(l => l.ReadyByRepresentative == false && l.RepresentativeId == repId);

                if (liquidation is null)
                {
                    liquidation = new Liquidation()
                    {
                        RepresentativeId = repId
                    };

                    await context.Liquidations.AddAsync(liquidation);

                }

                liquidation.Sales.Add(sale);
                sale.Liquidation = liquidation;

                await context.Sales.AddAsync(sale);
            }

            sale.Tickets.Add(ticketToAdd);
            ticketToAdd.Sale = sale;

            await context.Tickets.AddAsync(ticketToAdd);

            await context.SaveChangesAsync();

            //it will still buy the tickets, but redirect to the pick Excursion button
            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }

        //works!
        [HttpGet]
        public async Task<IActionResult> TicketCheckOut(TicketCreateInputModel model)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);
            //these 3 are indeed necessary!
            var sale = await context
                .Sales
                .Include(s => s.Tickets)
                .ThenInclude(t => t.Excursion)
                .ThenInclude(e => e.ExcursionType)
                .FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);


            var inputModel = new SaleCreateInputModel()
            {
                SaleId = sale.Id
            };

            inputModel.TicketsViews = sale.Tickets.Select(t => new SaleCreateTicketViewModel
            {
                TicketId = t.Id,
                AdultCount = t.AdultCount,
                ChildCount = t.ChildrenCount,
                ExcursionName = t.Excursion.ExcursionType.Name,
                PricePerAdult = t.Excursion.PricePerAdult,
                PricePerChild = t.Excursion.PricePerChild,
                Discount = t.Discount

            })
                .ToList();

            inputModel.TotalSumAfterDiscount = inputModel.TicketsViews.Any() ? inputModel.TicketsViews.Sum(t => t.TotalSum) : 0;

            return View(inputModel);
        }


        //this works too
        [HttpPost]
        public async Task<IActionResult> TicketCheckOut(SaleCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);

            model.IsInvalid = model.TotalAccumulated != model.TotalSumAfterDiscount;
            if (model.IsInvalid)
            {
                var secondSale = await context
              .Sales
              .Include(s => s.Tickets)
              .ThenInclude(t => t.Excursion)
              .ThenInclude(e => e.ExcursionType)
              .FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);


                model.IsInvalid = true;
                model.TicketsViews = secondSale.Tickets.Select(t => new SaleCreateTicketViewModel
                {
                    TicketId = t.Id,
                    AdultCount = t.AdultCount,
                    ChildCount = t.ChildrenCount,
                    ExcursionName = t.Excursion.ExcursionType.Name,
                    PricePerAdult = t.Excursion.PricePerAdult,
                    PricePerChild = t.Excursion.PricePerChild,
                    Discount = t.Discount,

                })
              .ToList();

                return this.View(model);
            }

            var sale = await context.Sales.FirstOrDefaultAsync(s => s.Id == model.SaleId);

            sale.Cash = model.Cash;
            sale.CreditCard = model.CreditCard;
            sale.TotalPrice = model.TotalSumAfterDiscount;
            sale.IsFinalized = true;
            context.Update(sale);
            await context.SaveChangesAsync();

            return View("/Areas/Finance/Views/Sales/ReceiptForm.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveAllPending([FromForm(Name = "SaleId")] int saleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Representatives.FirstOrDefaultAsync(r => r.UserId == userId);

            var sale = await context.Sales.Include(s => s.Tickets).FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);

            context.Tickets.RemoveRange(sale.Tickets);
            context.Sales.Remove(sale);
            await context.SaveChangesAsync();

            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }

        //ok
        public IActionResult TicketSearchById()
        {
            return View();
        }

        //this is working 
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
                    x.Discount,
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
                Discount = ticket.Discount
            };
            model.SumToRefund = (model.MaxAdultCount * model.PricePerAdult + model.MaxChildrenCount * model.PricePerChild) * (100 - ticket.Discount) / 100.00m;
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

            //here i forgot the discount!
            var cashToRefund =
                (model.AdultToRefund * model.PricePerAdult + model.ChildrenToRefund * model.PricePerChild)
                * (100m - ticket.Discount) / 100.00m;


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
