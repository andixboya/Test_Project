

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
            bool isSuccessful = await this.excursionServices.ExcursionTypeCreate(model);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult AddLanguage()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLanguage(LanguageAddInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("AddLanguage", "ExcursionOperator", model);
            }
            bool IsValid = await languageServices.LanguageCreate(model);

            return this.Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateExcursion()
        {
            var inputModel = new ExcursionCreateInputModel()
            {
                ExcursionTypes = await excursionServices.ExcursionTypesGetAll().ToListAsync(),
                Languages = await languageServices.GetAll().ToListAsync()
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
                model.ExcursionTypes = await excursionServices.ExcursionTypesGetAll().ToListAsync();
                model.Languages = await languageServices.GetAll().ToListAsync();

                return this.View(model);
            }


            bool isValid = await excursionServices.ExcursionCreate(model);

            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsExcursion([FromQuery(Name = "Id")] int id)
        {
            var username = User.Identity.Name;
            var serviceModel = await excursionServices.ExcursionGetDetails(id, username);

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
            bool isDeleted = await this.excursionServices.ExcursionDelete(id);
            return Redirect("/Excursion/ExcursionOperator/ViewAllExcursions");
        }

        //TODO: fix the edit !
        [HttpGet]
        public async Task<IActionResult> EditExcursion(int id)
        {
            var isPresent = await this.excursionServices.ContainsExcursion(id);

            if (isPresent is false)
            {
                //TODO: return with a message, no such excursion found
                return this.Redirect("/Home/Index");
            }

            var excursionToEdit = await excursionServices.ExcursionGetById(id);

            return this.View(excursionToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditExcursion(ExcursionCreateInputModel model)
        {
            model.LastUpdatedBy = User.Identity.Name;
            bool isValid = await excursionServices.ExcursionEdit(model);

            //TODO: here maybe a proper error throw

            return this.Redirect($"/Excursion/ExcursionOperator/DetailsExcursion?id={model.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllExcursions()
        {
            var excursions = await this.excursionServices.ExcursionViewAll();
            return this.View(excursions);
        }
    }
}
