

namespace ACTO.Web.ViewModels.Excursion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class ExcursionDetailsViewModel
    {
        
        public int Id { get; set; }
        public int ExcursionTypeId { get; set; }
        public ExcursionTypeViewModel ExcursionType { get; set; }

        public decimal Price { get; set; }
        
        public string StartingPoint { get; set; }

        public string EndPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }

        public int TouristCapacity { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<LanguageViewModel> Languages { get; set; }

    }
}
