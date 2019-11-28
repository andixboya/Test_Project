

namespace ACTO.Services.Models
{
    using ACTO.Data.Models.Excursions;
    using ACTO.Web.InputModels.Excursions;
    using AutoMapper;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ExcursionServiceModel : IMapFrom<ExcursionCreateInputModel>, IMapTo<Excursion>, IHaveCustomMappings
    {
        public ExcursionServiceModel()
        {
            this.LanguageIds = new List<int>();
            this.ExcursionTypes = new List<ExcursionTypeServiceModel>();
            this.Languages = new List<LanguageServiceModel>();
        }

        public int Id { get; set; }
        public int ExcursionTypeId { get; set; }
        public decimal PricePerAdult { get; set; }
        public decimal PricePerChild { get; set; }
        public string StartingPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int TouristCapacity { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ExtraSpot { get; set; }

        public List<int> LanguageIds { get; set; }
        public List<ExcursionTypeServiceModel> ExcursionTypes { get; set; }
        public List<LanguageServiceModel> Languages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ExcursionCreateInputModel, ExcursionServiceModel>()
                .ForMember(d => d.PricePerChild, y => y.MapFrom(s => s.ChildPrice))
                .ForMember(d => d.PricePerAdult, y => y.MapFrom(s => s.Price));
        }
    }
}
