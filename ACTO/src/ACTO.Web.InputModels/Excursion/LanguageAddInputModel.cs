

namespace ACTO.Web.InputModels.Excursion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class LanguageAddInputModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Language must be at least 33 symbols.")]
        [MaxLength(30, ErrorMessage = "Language must be below 30 symbols.")]
        public string Name { get; set; }
    }
}
