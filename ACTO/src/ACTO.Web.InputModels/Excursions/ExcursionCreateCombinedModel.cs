
namespace ACTO.Web.InputModels.Excursions
{
    using ACTO.Web.ViewModels.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ExcursionCreateCombinedModel
    {
        public ExcursionCreateViewModel ViewModel{ get; set; }
        public ExcursionCreateInputModel InputModel { get; set; }
    }
}
