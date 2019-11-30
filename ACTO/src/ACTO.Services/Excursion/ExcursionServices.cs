

namespace ACTO.Services.Excursion
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using ACTO.Web.ViewModels.Tickets;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public async Task<bool> ExcursionTypeCreate(ExcursionTypeCreateInputModel model)
        {
            var excursionTypeToAdd = new ExcursionType()
            {
                Name = model.Name
            };
            await context.ExcursionTypes.AddAsync(excursionTypeToAdd);
            await context.SaveChangesAsync();

            return true;
        }

        public IQueryable<ExcursionTypeViewModel> ExcursionTypesGetAll()
        {
            //this is iqueriable, it runs async! 
            return this.context.ExcursionTypes
                .Select(e => new ExcursionTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name
                });
        }

        public async Task<bool> ExcursionCreate(ExcursionCreateInputModel model)
        {
            var validLanguageIds = new HashSet<int>(await languageServices.GetAll().Select(l => l.Id).ToListAsync());

            var excursionToAdd = new Excursion()
            {
                Arrival = model.Arrival,
                Departure = model.Departure,
                LastUpdated = model.LastUpdated,
                TouristCapacity = model.TouristCapacity,
                LastUpdatedBy = model.LastUpdatedBy,
                EndPoint = model.EndPoint,
                PricePerAdult = model.Price,
                PricePerChild = model.ChildPrice,
                StartingPoint = model.StartingPoint,
                ExcursionTypeId = model.ExcursionTypeId


            };

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

        public async Task<bool> ExcursionDelete(int id)
        {
            var excursionToDelete = await context.Excursions.Include(e => e.LanguageExcursions).Include(e => e.SoldTickets).ThenInclude(t => t.Refunds).FirstOrDefaultAsync(e => e.Id == id);

            context.LanguageExcursions.RemoveRange(excursionToDelete.LanguageExcursions);
            context.Refunds.RemoveRange(excursionToDelete.SoldTickets.SelectMany(x => x.Refunds));
            context.Tickets.RemoveRange(excursionToDelete.SoldTickets);

            context.Excursions.Remove(excursionToDelete);
            await context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> ExcursionEdit(ExcursionCreateInputModel model)
        {
            var modelToEdit = await context.
                Excursions
                .Include(e => e.LanguageExcursions)
                .Include(e => e.ExcursionType)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            context.LanguageExcursions.RemoveRange(modelToEdit.LanguageExcursions);

            //WHY ON EARTH DID THIS THING SCREWED EVERYTHING UP? ( I HAVE REFERENCES!!)
            modelToEdit.LanguageExcursions.Clear();


            var excursionTypeToBreak = await context.ExcursionTypes.FindAsync(modelToEdit.ExcursionTypeId);
            excursionTypeToBreak.Excursions.Remove(modelToEdit);

            modelToEdit.LanguageExcursions = model.LanguageIds.Select(l => new LanguageExcursion
            {
                Excursion = modelToEdit,
                LanguageId = l
            })
                .ToList();

            modelToEdit.LastUpdated = DateTime.UtcNow;
            modelToEdit.LastUpdatedBy = model.LastUpdatedBy;
            modelToEdit.PricePerAdult = model.Price;
            modelToEdit.StartingPoint = model.StartingPoint;
            modelToEdit.TouristCapacity = model.TouristCapacity;
            modelToEdit.Arrival = model.Arrival;
            modelToEdit.Departure = model.Departure;
            modelToEdit.EndPoint = model.EndPoint;
            modelToEdit.ExcursionTypeId = model.ExcursionTypeId;

            await context.LanguageExcursions.AddRangeAsync(modelToEdit.LanguageExcursions);
            await context.SaveChangesAsync();


            return true;
        }

        public async Task<List<ExcursionSingleSearchView>> ExcursionViewAll()
        {
            var excursionViews = await context.Excursions
               .Select(e => new ExcursionSingleSearchView()
               {
                   Capacity = e.TouristCapacity,
                   Id = e.Id,
                   Name = e.ExcursionType.Name
               })
               .ToListAsync();

            return excursionViews;
        }

        public async Task<bool> ContainsExcursion(int id)
        {
            return await context.Excursions.AnyAsync(e => e.Id == id);
        }

        public async Task<ExcursionCreateInputModel> ExcursionGetById(int id)
        {
            //we don`t include the types and language, because of the multiple-choice select!
            var excursion = await context.Excursions.FindAsync(id);

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
                Languages = await languageServices.GetAll().Select(l => new LanguageViewModel()
                {
                    Id = l.Id,
                    Name = l.Name
                })
                .ToListAsync(),

                ExcursionTypes = await context.ExcursionTypes.Select(e => new ExcursionTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name
                })
                .ToListAsync()
            };

            return (inputModel);
        }

        public async Task<TicketPickExcursionViewModel> GetExcursionsForTicketCreateView()
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

            return excursionPickViewModel;
        }

        public async Task<List<LanguageViewModel>> GetLanguages(int excursionId)
        {
            return await languageServices.GetAllByExcursionId(excursionId).ToListAsync();
        }
    }
}
