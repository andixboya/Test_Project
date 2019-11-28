
namespace ACTO.Services.Models
{
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ExcursionTypeServiceModel : IMapFrom<ExcursionTypeCreateInputModel>, IMapTo<ExcursionType>,
                                             IMapFrom<ExcursionType>, IMapTo<ExcursionTypeViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
