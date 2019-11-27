

namespace ACTO.Web.Areas.Excursion.Controllers
{

    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    [Area("Excursion")]
    public class ExcursionOperatorController : Controller
    {
        private readonly ACTODbContext context;
        public ExcursionOperatorController(ACTODbContext db)
        {
            this.context = db;
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
            var excursionTypeToAdd = new ExcursionType()
            {
                Name = model.Name
            };
            await context.ExcursionTypes.AddAsync(excursionTypeToAdd);
            await context.SaveChangesAsync();

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

            var languageToAdd = new Language()
            {
                Name = model.Name
            };

            await context.LanguageTypes.AddAsync(languageToAdd);
            await context.SaveChangesAsync();

            return this.Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateExcursion()
        {
            var inputModel = new ExcursionCreateInputModel()
            {
                ExcursionTypes = await context.ExcursionTypes.Select(e => new ExcursionTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name
                })
                 .ToListAsync(),
                Languages = await context.LanguageTypes.Select(l => new LanguageViewModel()
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToListAsync()
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
                model.ExcursionTypes = await context.ExcursionTypes.Select(e => new ExcursionTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name
                })
                 .ToListAsync();

                model.Languages = await context.LanguageTypes.Select(l => new LanguageViewModel()
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToListAsync();

                //TODO:
                //forgot how this... return to form with data was done... 
                return this.View(model);
            }
            var validLanguageIds = new HashSet<int>(await context.LanguageTypes.Select(l => l.Id).ToListAsync());

            var newExcursion = new Excursion()
            {
                Arrival = model.Arrival,
                Departure = model.Departure,
                EndPoint = model.EndPoint,
                ExcursionTypeId = model.ExcursionTypeId,
                LastUpdated = model.LastUpdated,
                LastUpdatedBy = model.LastUpdatedBy,
                PricePerAdult = model.Price,
                PricePerChild = model.ChildPrice,
                StartingPoint = model.StartingPoint,
                TouristCapacity = model.TouristCapacity,
                AvailableSpots = model.TouristCapacity
            };

            newExcursion.LanguageExcursions = model
                .LanguageIds.Where(l => validLanguageIds.Contains(l))
                    .Distinct()
                    .Select(id => new LanguageExcursion()
                    {
                        Excursion = newExcursion,
                        LanguageId = id
                    })
                    .ToList();

            await context.Excursions.AddAsync(newExcursion);
            await context.SaveChangesAsync();

            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsExcursion(int Id)
        {
            //todo, refactor the models below without include!
            var excursion = await context.Excursions
                .Include(l => l.LanguageExcursions)
                .Include(l => l.ExcursionType)
                .FirstOrDefaultAsync(e => e.Id == Id);
            if (excursion is null)
            {
                //TODO: return with a message, no such excursion found
                return this.Redirect("/Home/Index");
            }
            var languageIds = excursion.LanguageExcursions.Select(l => l.LanguageId).ToList();

            var languages = await context
                .LanguageTypes
                .Where(l => languageIds.Contains(l.Id))
                .ToListAsync();

            var viewModel = new ExcursionDetailsViewModel()
            {
                Id = excursion.Id,
                Arrival = excursion.Arrival,
                Departure = excursion.Departure,
                EndPoint = excursion.EndPoint,
                ExcursionType = new ExcursionTypeViewModel()
                {
                    Id = excursion.ExcursionType.Id,
                    Name = excursion.ExcursionType.Name
                },
                Languages = languages.Select(l => new LanguageViewModel
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToList(),

                LastUpdated = excursion.LastUpdated,
                LastUpdatedBy = excursion.LastUpdatedBy
                ,
                Price = excursion.PricePerAdult
                ,
                StartingPoint = excursion.StartingPoint,
                TouristCapacity = excursion.TouristCapacity
            };

            //pass on the model
            return this.View(viewModel);
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
