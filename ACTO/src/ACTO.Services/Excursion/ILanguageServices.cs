

namespace ACTO.Services.Excursion
{
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILanguageServices
    {
        Task<bool> LanguageCreate(LanguageAddInputModel model);

        IQueryable<LanguageViewModel> GetAll();

        IQueryable<LanguageViewModel> GetAllByExcursionId(int id);
    }
}
