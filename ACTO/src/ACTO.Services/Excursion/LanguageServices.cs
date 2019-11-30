

namespace ACTO.Services.Excursion
{
    using ACTO.Data;
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IQueryable<LanguageViewModel> GetAllByExcursionId(int id)
        {
            return context.LanguageTypes.Where(l => l.LanguageExcursions.Any(le => le.ExcursionId == id)).Select(x => new LanguageViewModel
            {
                Id = x.Id,
                Name = x.Name
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
