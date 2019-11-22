

namespace ACTO.Web.Areas.Excursion.Controllers
{

    using ACTO.Data;
    using ACTO.Data.Models.Excursion;
    using ACTO.Web.InputModels.Excursion;
    using ACTO.Web.ViewModels.Excursion;
    using Microsoft.AspNetCore.Authorization;
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
            var excursionCreateViewModel = new ExcursionCreateViewModel()
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

            var combinedModel = new CombinedExcursioViewAndInputModel()
            {
                ViewModel = excursionCreateViewModel
            };

            return this.View(combinedModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExcursion(CombinedExcursioViewAndInputModel model)
        {
            //TODO: validate before/after date to be meaningful + some bug with the requirement for date
            model.InputModel.LastUpdated = DateTime.UtcNow;
            model.InputModel.LastUpdatedBy = HttpContext.User.Identity.Name;

            var inputModel = model.InputModel;


            if (!ModelState.IsValid)
            {
                model.ViewModel = new ExcursionCreateViewModel()
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
                //TODO:
                //forgot how this... return to form with data was done... 
                return this.View(model);
            }
            var validLanguageIds = new HashSet<int>(await context.LanguageTypes.Select(l => l.Id).ToListAsync());

            var newExcursion = new Excursion()
            {
                Arrival = inputModel.Arrival,
                Departure = inputModel.Departure,
                EndPoint = inputModel.EndPoint,
                ExcursionTypeId = inputModel.ExcursionTypeId,
                LastUpdated = inputModel.LastUpdated,
                LastUpdatedBy = inputModel.LastUpdatedBy,
                Price = inputModel.Price,
                StartingPoint = inputModel.StartingPoint,
                TouristCapacity = inputModel.TouristCapacity,
            };

            newExcursion.LanguageExcursions = inputModel
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
                Price = excursion.Price
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
            var excursionToDelete = await context.Excursions.FindAsync(id);

            context.Excursions.Remove(excursionToDelete);
            await context.SaveChangesAsync();
            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }
        [HttpGet]
        public async Task<IActionResult> EditExcursion(int Id)
        {
            var excursion = await context.Excursions
                .Include(l => l.LanguageExcursions)
                .Include(l => l.ExcursionType)
                .FirstOrDefaultAsync(e => e.Id == Id);
            if (excursion is null)
            {
                //TODO: return with a message, no such excursion found
                return this.Redirect("/Home/Index");
            }



            var excursionCreateViewModel = new ExcursionCreateViewModel()
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

            var combinedModel = new CombinedExcursioViewAndInputModel()
            {
                ViewModel = excursionCreateViewModel,
                InputModel = new ExcursionCreateInputModel()
                {
                    Id = excursion.Id,
                    Arrival = excursion.Arrival,
                    Departure = excursion.Departure,
                    EndPoint = excursion.EndPoint,
                    LastUpdated = excursion.LastUpdated,
                    LastUpdatedBy = excursion.LastUpdatedBy,
                    Price = excursion.Price,
                    StartingPoint = excursion.StartingPoint,
                    TouristCapacity = excursion.TouristCapacity
                }
            };


            return this.View(combinedModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditExcursion(CombinedExcursioViewAndInputModel model)
        {
            var modelToEdit = await context.
                Excursions
                .Include(x=> x.LanguageExcursions)
                .FirstOrDefaultAsync(x=> x.Id==model.InputModel.Id);    

            context.LanguageExcursions.RemoveRange(modelToEdit.LanguageExcursions);
            var excursionTypeToBreak= await context.ExcursionTypes.FindAsync(modelToEdit.ExcursionTypeId);
            excursionTypeToBreak.Excursions.Remove(modelToEdit);
            context.Update(excursionTypeToBreak);

            modelToEdit.LanguageExcursions = model.InputModel.LanguageIds.Select(l => new LanguageExcursion
            {
                Excursion = modelToEdit,
                LanguageId = l
            })
                .ToList();
            modelToEdit.LastUpdated = DateTime.UtcNow;
            modelToEdit.LastUpdatedBy = HttpContext.User.Identity.Name;
            modelToEdit.Price = model.InputModel.Price;
            modelToEdit.StartingPoint = model.InputModel.StartingPoint;
            modelToEdit.TouristCapacity = model.InputModel.TouristCapacity;
            modelToEdit.Arrival = model.InputModel.Arrival;
            modelToEdit.Departure = model.InputModel.Departure;
            modelToEdit.EndPoint = model.InputModel.EndPoint;
            modelToEdit.ExcursionTypeId = model.InputModel.ExcursionTypeId;

            context.Update(modelToEdit);
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
