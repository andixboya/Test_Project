

namespace ACTO.Web.Areas.Finance.Controllers
{
    using ACTO.Data;
    using ACTO.Web.ViewModels.Liquidations;
    using ACTO.Web.ViewModels.Refund;
    using ACTO.Web.ViewModels.Tickets;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Area("Finance")]
    public class CashierController : Controller
    {
        private readonly ACTODbContext context;
        public CashierController(ACTODbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> LiquidationPendingAll()
        {
            var pendingLiquidations = await context
                .Liquidations
                .Where(l => l.ReadyByCashier == false && l.ReadyByRepresentative == true)
                .Select(l => new LiquidationPickViewModel()
                {
                    Id = l.Id,
                    Cash = l.Cash,
                    CreditCard = l.CreditCard,
                    RepName = l.Representative.UserName
                })
                .ToListAsync();

            return this.View(pendingLiquidations);
        }


        [HttpGet]
        public async Task<IActionResult> LiquidationConfirm(int id)
        {
            var liquidaitonToApprove = await context.Liquidations.Select(l => new LiquidationApproveByCashierViewModel()
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
                Cash = l.Cash,
                CreditCard = l.CreditCard,
                TotalOwned = l.TotalSum
            })
                .FirstOrDefaultAsync(model => model.LiquidationId == id);



            //will show the sales of the rep ( with refunds) + totalOwned, + how he wants to pay ( added by him)
            //and just approve it , if everything is ok.

            //var liquidationToConfirm= await context.Liquidations.
            return this.View(liquidaitonToApprove);
        }

        [HttpPost]
        public async Task<IActionResult> LiquidationConfirm(LiquidationApproveByRepViewModel input)
        {
            var liquidationToConfirm = await context.Liquidations.FirstOrDefaultAsync(l => l.Id == input.LiquidationId);
            liquidationToConfirm.ReadyByCashier = true;
            await context.SaveChangesAsync();

            return Redirect("/Home/Index");
        }


        [HttpPost]
        public async Task<IActionResult> LiquidationDenie([FromForm(Name = "LiquidationId")] int id)
        {
            //just turn the bool approved by rep. to false and probably nullify his way ot payment (cash / cc)
            //, because it most likely is wrong.
            var liquidationToDenie = await context.Liquidations.FirstOrDefaultAsync(l => l.Id == id);

            liquidationToDenie.Cash = 0;
            liquidationToDenie.CreditCard = 0;
            liquidationToDenie.ReadyByRepresentative = false;

            await context.SaveChangesAsync();
            return Redirect("/Home/Index");
        }






        //1 with option to search by name or by id of the representative

        //1 list, of each sale (which shows how much he owes (credit cards/ cash) 


        //1 for marking them all and giving them s liquidation with actual amount (how much has gone in and that`s it)

        //1 (report) after that.... just  reports, perhaps, which is just listing of... liquidations  (which has sales, which has tickets)
        //

        //TODO: also fix the limiter for each excursion, because that is important
        //TODO:throw error if a ticket is refunded to the max ( 0 , 0)  , same for excursion (just block them out)
        //TODO: that `s about all of it...


        //rest is services!!!!

        //+ some unit tests and you are done! 
    }
}
