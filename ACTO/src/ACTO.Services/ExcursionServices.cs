

namespace ACTO.Services
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Services.Models;
    using ACTO.Web.ViewModels.Excursions;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ExcursionServices : IExcursionServices
    {
        private readonly ACTODbContext context;
        private readonly ILanguageServices languageServices;

        public ExcursionServices(ACTODbContext context, ILanguageServices languageServices)
        {
            this.languageServices = languageServices;
            this.context = context;
        }
        public async Task<bool> ExcursionTypeCreate(ExcursionTypeServiceModel model)
        {
            var excursionToAdd = model.To<ExcursionType>();
            await context.ExcursionTypes.AddAsync(excursionToAdd);

            return true;
        }

        public IQueryable<ExcursionTypeServiceModel> ExcursionTypesGetAll()
        {
            //this is iqueriable, it runs async! 
            return this.context.ExcursionTypes.To<ExcursionTypeServiceModel>();
        }

        public async Task<bool> ExcursionCreate(ExcursionServiceModel model)
        {
            var validLanguageIds = new HashSet<int>(await languageServices.GetAll().Select(l => l.Id).ToListAsync());
            var excursionToAdd = model.To<Excursion>();

            excursionToAdd.LanguageExcursions = model
               .LanguageIds.Where(l => validLanguageIds.Contains(l))
                   .Distinct()
                   .Select(id => new LanguageExcursion()
                   {
                       Excursion = excursionToAdd,
                       LanguageId = id
                   })
                   .ToList();

            await context.Excursions.AddAsync(excursionToAdd);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ExcursionDetailsViewModel> ExcursionGetDetails(int id, string username)
        {
            var excursion = await context
                .Excursions
                .Select(e => new ExcursionDetailsViewModel()
                {
                    Id = e.Id,
                    Arrival = e.Arrival,
                    Departure = e.Departure,
                    EndPoint = e.EndPoint,
                    ExcursionTypeId = e.ExcursionTypeId,
                    Languages = e.LanguageExcursions.Select(l => new LanguageViewModel()
                    {
                        Id = l.Language.Id,
                        Name = l.Language.Name
                    })
               .ToList(),
                    ExcursionType = new ExcursionTypeViewModel()
                    {
                        Id = e.ExcursionType.Id,
                        Name = e.ExcursionType.Name
                    },

                    LastUpdated = DateTime.UtcNow,
                    LastUpdatedBy = username,
                    PricePerAdult = e.PricePerAdult,
                    PricePerChild = e.PricePerChild,
                    StartingPoint = e.StartingPoint,
                    TouristCapacity = e.TouristCapacity

                })
                .FirstOrDefaultAsync(e => e.Id == id);

            return excursion;
        }

    }
}
