

namespace ACTO.Web.InputModels.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class ExcursionTypeCreateInputModel 
    {

        [Required]
        [MinLength(3,ErrorMessage ="Name of excursion must be at least 3 symbols.")]
        [StringLength(30,ErrorMessage = "Name of excursion must be below 30 symbols.")]
        public string Name { get; set; }
    }
}
