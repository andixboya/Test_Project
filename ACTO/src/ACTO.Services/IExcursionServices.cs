

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

    public interface IExcursionServices
    {
        Task<bool> ExcursionTypeCreate(ExcursionTypeCreateInputModel model);

        IQueryable<ExcursionTypeViewModel> ExcursionTypesGetAll();

        //one left. to do!

        Task<bool> ExcursionCreate(ExcursionCreateInputModel model);
        Task<ExcursionDetailsViewModel> ExcursionGetDetails(int id, string username);

        Task<bool> ExcursionDelete(int id);

        Task<bool> ExcursionEdit(ExcursionCreateInputModel model);

        Task<List<ExcursionSingleSearchView>> ExcursionViewAll();

        Task<bool> ContainsExcursion(int id);

        Task<ExcursionCreateInputModel> ExcursionGetById(int id);
    }
}
