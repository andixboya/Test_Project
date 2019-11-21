
namespace ACTO.Web.ViewModels.Excursion
{
    using ACTO.Web.InputModels.Excursion;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CombinedExcursioViewAndInputModel
    {
        public ExcursionCreateViewModel ViewModel{ get; set; }
        public ExcursionCreateInputModel InputModel { get; set; }
    }
}
