

namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class LanguageExcursion
    {
        public int LanguageId { get; set; }
        public LanguageType Language { get; set; }

        public int ExcursionId { get; set; }

        public Excursion Excursion { get; set; }

    }
}
