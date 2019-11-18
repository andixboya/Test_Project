

namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LanguageType :BaseModel<int>
    {
        public string Name { get; set; }
        public ICollection<LanguageExcursion> Excursions { get; set; }
    }
}
