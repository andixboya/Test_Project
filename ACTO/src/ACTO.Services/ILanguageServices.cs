

namespace ACTO.Services
{
    using ACTO.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILanguageServices
    {
        Task<bool> LanguageCreate(LanguageServiceModel model);

        IQueryable<LanguageServiceModel> GetAll();
    }
}
