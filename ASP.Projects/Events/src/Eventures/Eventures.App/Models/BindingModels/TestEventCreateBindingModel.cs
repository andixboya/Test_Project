

namespace Eventures.App.Models.BindingModels
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class TestEventCreateBindingModel :IValidatableObject
    {
        [Required (ErrorMessage ="Name must be filled in!")]
        [StringLength(30,ErrorMessage ="Name must be between 3 and 30 symbols.",MinimumLength =3)]
        [Display(Name="Event name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Place must be filled!")]
        [StringLength(30, ErrorMessage = "Place must be between 3 and 30 symbols.", MinimumLength = 3)]
        [Display(Name = "Place of the event")]

        public string Place { get; set; }

        
        [DataType( DataType.Date,ErrorMessage ="Error with the setting of the date")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Error with the setting of the date")]
        public DateTime End { get; set; }


        public int TotalTickets { get; set; }


        public decimal PricePerTicket { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Start.Date>=this.Start.Date)     
            {
                yield return new ValidationResult("End Date must be later than start!");
            }

        }
    }
}
