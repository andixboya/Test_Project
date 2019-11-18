
namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Excursion :BaseModel<int>
    {
        public Excursion()
        {
            this.LanguageExcursions = new HashSet<LanguageExcursion>();
        }
        public int ExcursionTypeId { get; set; }
        public ExcursionType ExcursionType { get; set; }
        public decimal Price { get; set; }

        public string StartingPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public int TouristCapacity { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<LanguageExcursion> LanguageExcursions { get; set; }

    }
}
