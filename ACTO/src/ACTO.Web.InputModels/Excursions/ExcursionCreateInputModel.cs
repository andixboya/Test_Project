

namespace ACTO.Web.InputModels.Excursions
{
    using ACTO.Web.ViewModels.Excursions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Threading.Tasks;

    public class ExcursionCreateInputModel
    {
        public ExcursionCreateInputModel()
        {
            this.LanguageIds = new List<int>();
            this.ExcursionTypes = new List<ExcursionTypeViewModel>();
            this.Languages = new List<LanguageViewModel>();
        }

        public int Id { get; set; }

        [Required]
        public int ExcursionTypeId { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal ChildPrice { get; set; }

        [MinLength(3)]
        [Required]
        public string StartingPoint { get; set; }

        [MinLength(3)]

        [Required]
        public string EndPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]

        public DateTime Departure { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]

        public DateTime Arrival { get; set; }

        [Range(0, 5000)]
        public int TouristCapacity { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ExtraSpot { get; set; }

        public List<int> LanguageIds { get; set; }


        public List<ExcursionTypeViewModel> ExcursionTypes { get; set; }

        public List<LanguageViewModel> Languages { get; set; }

    }
}
