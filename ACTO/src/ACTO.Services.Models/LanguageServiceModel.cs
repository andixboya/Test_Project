

namespace ACTO.Services.Models
{
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using ACTO.Web.ViewModels.Excursions;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LanguageServiceModel :IMapFrom<LanguageAddInputModel> , IMapTo<Language> 
                                     , IMapFrom<Language>, IMapTo<LanguageViewModel>
    {

        public LanguageServiceModel()
        {
            this.LanguageExcursions = new List<LanguageExcursion>();
            this.Tickets = new List<Ticket>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<LanguageExcursion> LanguageExcursions { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
