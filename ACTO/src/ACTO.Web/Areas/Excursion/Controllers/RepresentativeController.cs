

namespace ACTO.Web.Areas.Excursion.Controllers
{
    using ACTO.Data;
    using ACTO.Web.ViewModels.Tickets;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    [Area("Excursion")]
    public class RepresentativeController : Controller
    {
        private readonly ACTODbContext context;
        public RepresentativeController(ACTODbContext db)
        {
            this.context = db;
        }

        public async Task<IActionResult> TicketPickExcursion()
        {
            var excursionViewModels = await context.Excursions.Select(e => new TicketPickExcursionViewModel()
            {
                AvailableSpots = e.AvailableSpots,
                Id = e.Id,
                Name = e.ExcursionType.Name
            })
                .ToListAsync();


            return View(excursionViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> TicketCreate(int id)
        {
            var excursion = await context.Excursions.FindAsync(id);

            if (excursion is null)
            {
                //TODO: throw proper error.
                this.Redirect("/Home/Index");
            }




            return View(excursion);
        }

    }
}
