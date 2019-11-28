

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
    using Stopify.Services.Mapping;

    public class LanguageServices : ILanguageServices
    {
        private readonly ACTODbContext context;

        public LanguageServices(ACTODbContext context)
        {
            this.context = context;
        }

        public IQueryable<LanguageServiceModel> GetAll()
        {
            return this.context.LanguageTypes.To<LanguageServiceModel>();
        }

        public async Task<bool> LanguageCreate(LanguageServiceModel model)
        {
            var languageToAdd = model.To<Language>();

            await context.LanguageTypes.AddAsync(languageToAdd);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
