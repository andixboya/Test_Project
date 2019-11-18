

namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ExcursionType : BaseModel<int>
    {
        public string Name { get; set; }

        public ICollection<Excursion> Excursions { get; set; }
    }
}
