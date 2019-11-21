

namespace ACTO.Web.InputModels.Excursion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class ExcursionCreateInputModel
    {

        public ExcursionCreateInputModel()
        {
            this.LanguageIds = new List<int>();
        }


        [Required]
        public int ExcursionTypeId { get; set; }

        public decimal Price { get; set; }

        [MinLength(3)]
        [Required]
        public string StartingPoint { get; set; }

        [MinLength(3)]

        [Required]
        public string EndPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Departure { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Arrival { get; set; }

        [Range(0, 5000)]
        public int TouristCapacity { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<int> LanguageIds { get; set; }

    }
}
