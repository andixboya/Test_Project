
namespace ACTO.Data.Models.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class Excursion :BaseModel<int>
    {
        public Excursion()
        {
            this.LanguageExcursions = new HashSet<LanguageExcursion>();
        }
        [Required]
        public int ExcursionTypeId { get; set; }
        public ExcursionType ExcursionType { get; set; }
        
        [Range(typeof(decimal),"0.00m", "79228162514264337593543950335M")]
        public decimal Price { get; set; }

        public decimal ChildPrice { get; set; }

        [Required]
        public string StartingPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        [Required]
        public DateTime Departure { get; set; }
        
        [Required]
        public DateTime Arrival { get; set; }

        [Range(0,5000)]
        public int TouristCapacity { get; set; }

        public int AvailableSpots { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<LanguageExcursion> LanguageExcursions { get; set; }

        public ICollection<Ticket> SoldTickets { get; set; }

    }
}
