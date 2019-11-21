

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
                ViewModel = excursionCreateViewModel,
                InputModel = new ExcursionCreateInputModel()
                {

                    Departure = DateTime.UtcNow.AddDays(5)
                    ,
                    Arrival = DateTime.UtcNow.AddDays(15)
                }
            };

            return this.View(combinedModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateExcursion(CombinedExcursioViewAndInputModel model)
        {
            if (!ModelState.IsValid)
            {
                //TODO:
                //forgot how this... return to form with data was done... 
                return this.RedirectToAction("CreateExcursion", "ExcursionOperator", model);
            }




            return this.RedirectToAction("/Home/Index");
        }

    }
}
