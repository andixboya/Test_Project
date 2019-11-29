

namespace ACTO.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Services.Models;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using Stopify.Services.Mapping;

    public class LanguageServices : ILanguageServices
    {
        private readonly ACTODbContext context;

        public LanguageServices(ACTODbContext context)
        {
            this.context = context;
        }

        public IQueryable<LanguageViewModel> GetAll()
        {
            return this.context.LanguageTypes
                .Select(lt => new LanguageViewModel()
                {
                    Id = lt.Id,
                    Name = lt.Name
                });
        }

        public async Task<bool> LanguageCreate(LanguageAddInputModel model)
        {
            var languageToAdd = new Language()
            {
                Name = model.Name
            };


            await context.LanguageTypes.AddAsync(languageToAdd);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
