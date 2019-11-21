

namespace ACTO.Data.Models.Excursion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class Language :BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
        public ICollection<LanguageExcursion> Excursions { get; set; }
    }
}
