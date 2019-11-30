

namespace ACTO.Web.Areas.Excursion.Controllers
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Data.Models.Finance;
    using ACTO.Services.Excursion;
    using ACTO.Services.Others;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.InputModels.Finance;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Liquidations;
    using ACTO.Web.ViewModels.Refund;
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
        private readonly ITicketServices ticketServices;


        //should have started from the liquidation services... (which should have given commands, but w.e.)
        public RepresentativeController(ACTODbContext db, ITicketServices ticketServices)
        {
            this.ticketServices = ticketServices;
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
            var excursionPickViewModel = await this.ticketServices.GetPossibleExcursionsForTicket();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //for button which to reset the not sold ones.
            this.ViewData["AnyTickets"] = await this.ticketServices.HasPendingTickets(userId);
            return View(excursionPickViewModel);
        }

        //done. 
        [HttpGet]
        public async Task<IActionResult> TicketCreate(TicketPickExcursionViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var excursion = model.PickedExcursion;
            if (excursion is null)
            {
                //TODO: proper exception !
                this.Redirect("/Home/Index");
            }

            List<LanguageViewModel> spokenLanguages = await ticketServices.GetLanguagesForExcursion(excursion.Id);

            var input = await this.
                ticketServices
                .FillGetTicketCreateInputModel(excursion, spokenLanguages, userId);

            return this.View(input);
        }

        [HttpPost] //works too (decoupled it from ticket checkout) 
        public async Task<IActionResult> TicketCreate(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (model is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            bool IsCreated = await this.ticketServices.CreateTicket(model, userId);
            //i won`t be making any special checkup for customers (not worth it i think?) 

            return this.Redirect("/Excursion/Representative/TicketCheckOut");
        }

        [HttpPost] //works!
        public async Task<IActionResult> TicketCreateAnother(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                //TODO: proper error message
                return this.Redirect("/Home/Index");
            }

            if (model is null)
            {
                //TODO: add proper error message!
                return this.Redirect("/Home/Index");
            }

            bool IsCreated = await this.ticketServices.CreateTicket(model, userId);

            //it will still buy the tickets, but redirect to the pick Excursion button
            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }

        //works!
        [HttpGet]
        public async Task<IActionResult> TicketCheckOut(TicketCreateInputModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
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
            var rep = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);


            //we`ll use it for visualization or to return the model if it is invalid
            var sale = await context
              .Sales
              .Include(s => s.Tickets)
              .ThenInclude(t => t.Excursion)
              .ThenInclude(e => e.ExcursionType)
              .FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);


            model.IsInvalid = model.TotalAccumulated != model.TotalSumAfterDiscount;
            if (model.IsInvalid)
            {
                model.IsInvalid = true;
                model.TicketsViews = sale.Tickets.Select(t => new SaleCreateTicketViewModel
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

            sale.Cash = model.Cash;
            sale.CreditCard = model.CreditCard;
            sale.TotalPrice = model.TotalSumAfterDiscount;
            sale.IsFinalized = true;
            await context.SaveChangesAsync();

            //todo: implement this... but its not a priority!
            SaleReceiptFormView modelView = new SaleReceiptFormView()
            {

            };

            return View("/Areas/Finance/Views/Sales/ReceiptForm.cshtml", modelView);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveAllPending([FromForm(Name = "SaleId")] int saleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var sale = await context.Sales.Include(s => s.Tickets).FirstOrDefaultAsync(s => s.IsFinalized == false &&
                s.Liquidation.RepresentativeId == rep.Id);

            context.Tickets.RemoveRange(sale.Tickets);
            context.Sales.Remove(sale);
            await context.SaveChangesAsync();

            return Redirect("/Excursion/Representative/TicketPickExcursion");
        }


        public IActionResult TicketSearchById()
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


            await context.Refunds.AddAsync(refund);
            await context.SaveChangesAsync();

            return View("/Areas/Excursion/Views/Representative/Tickets/TicketRefundView.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> LiquidationPendingViewAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var liquidationIds = await context
                .Liquidations
                .Where(l => l.RepresentativeId == rep.Id
                && l.ReadyByRepresentative == false)
                .Select(l => l.Id)
                .ToListAsync();

            return View(liquidationIds);
        }

        [HttpGet]
        public async Task<IActionResult> LiquidationApprove(int id)
        {
            var liquidation = await context
                .Liquidations
                .Select(l => new LiquidationApproveByRepViewModel()
                {
                    LiquidationId = l.Id,
                    Tickets = l.Sales.SelectMany(s => s.Tickets)
                    .Select(t => new TicketViewModel()
                    {
                        AdultCount = t.AdultCount,
                        ChildCount = t.ChildrenCount,
                        Discount = t.Discount,
                        ExcursionName = t.Excursion.ExcursionType.Name,
                        PricePerAdult = t.Excursion.PricePerAdult,
                        PricePerChild = t.Excursion.PricePerChild,

                        Refunds = t.Refunds.Select(r => new RefundViewModel()
                        {
                            AdultCount = r.AdultCount,
                            Cash = r.Cash,
                            ChildCount = r.ChildCount,
                            CreditCard = r.CreditCard
                        })
                        .ToList()

                    }).ToList()

                })
                .FirstOrDefaultAsync(model => model.LiquidationId == id);

            if (!liquidation.Tickets.Any())
            {
                liquidation.TotalOwned = 0;
            }
            else
            {
                var refunds = liquidation.Tickets.SelectMany(t => t.Refunds).ToList();
                var refundSum = refunds.Any() ? refunds.Sum(r => r.Amount) : 0;

                liquidation.TotalOwned = liquidation.Tickets.Sum(t => t.PriceAfterDiscount) - refundSum;
            }

            return this.View(liquidation);
        }

        [HttpPost]
        public async Task<IActionResult> LiquidationApprove(LiquidationApproveByRepViewModel model)
        {
            if (model.TotalAccumulated != model.TotalOwned)
            {
                model = await context
                .Liquidations
                .Select(l => new LiquidationApproveByRepViewModel()
                {
                    LiquidationId = l.Id,
                    Tickets = l.Sales.SelectMany(s => s.Tickets)
                    .Select(t => new TicketViewModel()
                    {
                        AdultCount = t.AdultCount,
                        ChildCount = t.ChildrenCount,
                        Discount = t.Discount,
                        ExcursionName = t.Excursion.ExcursionType.Name,
                        PricePerAdult = t.Excursion.PricePerAdult,
                        PricePerChild = t.Excursion.PricePerChild,

                        Refunds = t.Refunds.Select(r => new RefundViewModel()
                        {
                            AdultCount = r.AdultCount,
                            Cash = r.Cash,
                            ChildCount = r.ChildCount,
                            CreditCard = r.CreditCard
                        })
                        .ToList()

                    }).ToList(),
                    IsInvalid = true
                })
                .FirstOrDefaultAsync(model => model.LiquidationId == model.LiquidationId);

                return this.View(model);
            }

            var liquidationToChange = await context.Liquidations.FindAsync(model.LiquidationId);
            liquidationToChange.Cash = model.Cash;
            liquidationToChange.CreditCard = model.CreditCard;
            liquidationToChange.ReadyByRepresentative = true;

            await context.SaveChangesAsync();

            return Redirect("/Home/Index");
        }
    }
}
