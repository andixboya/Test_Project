

namespace ACTO.Services
{
    using ACTO.Services.Models;
    using ACTO.Web.ViewModels.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IExcursionServices
    {
        Task<bool> ExcursionTypeCreate(ExcursionTypeServiceModel model);

        IQueryable<ExcursionTypeServiceModel> ExcursionTypesGetAll();

        Task<bool> ExcursionCreate(ExcursionServiceModel model);
        Task<ExcursionDetailsViewModel> ExcursionGetDetails(int id, string username);
    }
}
