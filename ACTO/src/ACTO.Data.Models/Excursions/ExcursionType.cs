

namespace ACTO.Data.Models.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class ExcursionType : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Excursion> Excursions { get; set; }
    }
}
