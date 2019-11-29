

namespace ACTO.Services
{
    using ACTO.Services.Models;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILanguageServices
    {
        Task<bool> LanguageCreate(LanguageAddInputModel model);

        IQueryable<LanguageViewModel> GetAll();
    }
}
