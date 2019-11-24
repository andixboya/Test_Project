

namespace ACTO.Data.Models.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class Language : BaseModel<int>
    {

        public Language()
        {
            this.LanguageExcursions = new HashSet<LanguageExcursion>();
            this.Tickets = new HashSet<Ticket>();
        }

        [Required]
        public string Name { get; set; }
        public ICollection<LanguageExcursion> LanguageExcursions { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
