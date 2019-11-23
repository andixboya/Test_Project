

namespace ACTO.Data.Models.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LanguageExcursion
    {
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public int ExcursionId { get; set; }

        public Excursion Excursion { get; set; }

    }
}
