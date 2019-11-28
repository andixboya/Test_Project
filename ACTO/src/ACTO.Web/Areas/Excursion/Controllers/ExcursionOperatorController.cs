

namespace ACTO.Web.Areas.Excursion.Controllers
{

    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Services;
    using ACTO.Services.Models;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    [Area("Excursion")]
    public class ExcursionOperatorController : Controller
    {
        private readonly ACTODbContext context;
        private readonly IExcursionServices excursionServices;
        private readonly ILanguageServices languageServices;
        public ExcursionOperatorController(ACTODbContext db, IExcursionServices excursionServices, ILanguageServices languageServices)
        {
            this.context = db;
            this.excursionServices = excursionServices;
            this.languageServices = languageServices;
        }

        //[Authorize(Roles ="Admin,ExcursionOperator")]
        [HttpGet]
        public IActionResult CreateExcursionType()
        {
            return this.View();
        }

        //[Authorize(Roles = "Admin,ExcursionOperator")]
        [HttpPost]
        public async Task<IActionResult> CreateExcursionType(ExcursionTypeCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                //we`ll return the input values i think
                this.RedirectToAction("CreateExcursionType", "ExcursionOperator", model);
            }
            var excursionServiceModel = model.To<ExcursionTypeServiceModel>();
            bool isSuccessful = await this.excursionServices.ExcursionTypeCreate(excursionServiceModel);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult AddLanguage()
        {
            return this.View();
        }

        public async Task<IActionResult> AddLanguage(LanguageAddInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("AddLanguage", "ExcursionOperator", model);
            }

            var languageServiceModel = model.To<LanguageServiceModel>();
            bool IsValid = await languageServices.LanguageCreate(languageServiceModel);

            return this.Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateExcursion()
        {
            var inputModel = new ExcursionCreateInputModel()
            {
                ExcursionTypes = await excursionServices.ExcursionTypesGetAll().To<ExcursionTypeViewModel>().ToListAsync(),
                Languages = await languageServices.GetAll().To<LanguageViewModel>().ToListAsync()
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExcursion(ExcursionCreateInputModel model)
        {
            //TODO: validate before/after date to be meaningful + some bug with the requirement for date
            model.LastUpdated = DateTime.UtcNow;
            model.LastUpdatedBy = HttpContext.User.Identity.Name;

            if (!ModelState.IsValid)
            {
                model.ExcursionTypes = await excursionServices.ExcursionTypesGetAll().To<ExcursionTypeViewModel>().ToListAsync();
                model.Languages = await languageServices.GetAll().To<LanguageViewModel>().ToListAsync();

                return this.View(model);
            }

            var serviceModel = model.To<ExcursionServiceModel>();
            bool isValid = await excursionServices.ExcursionCreate(serviceModel);

            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsExcursion([FromQuery(Name = "Id")] int id)
        {
            var username = User.Identity.Name;
            var serviceModel = await excursionServices.ExcursionGetDetails(id,username);

            if (serviceModel is null)
            {
                //TODO: return with a message, no such excursion found
                return this.Redirect("/Home/Index");
            }

         
            return this.View(serviceModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExcursion(int id)
        {
            var excursionToDelete = await context.Excursions.Include(e => e.LanguageExcursions).Include(e => e.SoldTickets).ThenInclude(t => t.Refunds).FirstOrDefaultAsync(e => e.Id == id);

            context.LanguageExcursions.RemoveRange(excursionToDelete.LanguageExcursions);
            context.Refunds.RemoveRange(excursionToDelete.SoldTickets.SelectMany(x => x.Refunds));
            context.Tickets.RemoveRange(excursionToDelete.SoldTickets);

            context.Excursions.Remove(excursionToDelete);
            await context.SaveChangesAsync();
            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }
        [HttpGet]
        public async Task<IActionResult> EditExcursion(int id)
        {
            var excursion = await context.Excursions
                .Include(l => l.LanguageExcursions)
                .Include(l => l.ExcursionType)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (excursion is null)
            {
                //TODO: return with a message, no such excursion found
                return this.Redirect("/Home/Index");
            }


            var inputModel = new ExcursionCreateInputModel()
            {
                Id = excursion.Id,
                Arrival = excursion.Arrival,
                Departure = excursion.Departure,
                EndPoint = excursion.EndPoint,
                LastUpdated = excursion.LastUpdated,
                LastUpdatedBy = excursion.LastUpdatedBy,
                Price = excursion.PricePerAdult,
                StartingPoint = excursion.StartingPoint,
                TouristCapacity = excursion.TouristCapacity,
                Languages = await context.LanguageTypes.Select(l => new LanguageViewModel()
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToListAsync(),
                ExcursionTypes = await context.ExcursionTypes.Select(e => new ExcursionTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name
                })
              .ToListAsync()
            };
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditExcursion(ExcursionCreateInputModel model)
        {
            var modelToEdit = await context.
                Excursions
                .Include(x => x.LanguageExcursions)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            context.LanguageExcursions.RemoveRange(modelToEdit.LanguageExcursions);
            var excursionTypeToBreak = await context.ExcursionTypes.FindAsync(modelToEdit.ExcursionTypeId);
            excursionTypeToBreak.Excursions.Remove(modelToEdit);

            modelToEdit.LanguageExcursions = model.LanguageIds.Select(l => new LanguageExcursion
            {

                Excursion = modelToEdit,
                LanguageId = l
            })
                .ToList();

            modelToEdit.LastUpdated = DateTime.UtcNow;
            modelToEdit.LastUpdatedBy = HttpContext.User.Identity.Name;
            modelToEdit.PricePerAdult = model.Price;
            modelToEdit.StartingPoint = model.StartingPoint;
            modelToEdit.TouristCapacity = model.TouristCapacity;
            modelToEdit.Arrival = model.Arrival;
            modelToEdit.Departure = model.Departure;
            modelToEdit.EndPoint = model.EndPoint;
            modelToEdit.ExcursionTypeId = model.ExcursionTypeId;


            await context.SaveChangesAsync();

            return this.Redirect($"/Excursion/ExcursionOperator/DetailsExcursion?id={modelToEdit.Id}");
        }

        public async Task<IActionResult> ViewAllExcursions()
        {
            var excursions = await context.Excursions
                .Select(e => new ExcursionSingleSearchView()
                {
                    Capacity = e.TouristCapacity,
                    Id = e.Id,
                    Name = e.ExcursionType.Name
                })
                .ToListAsync();


            return this.View(excursions);
        }
    }
}
