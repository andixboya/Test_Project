using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACTO.Web.ViewModels.Excursion
{
    public class ExcursionCreateViewModel
    {
        public ExcursionCreateViewModel()
        {
            this.ExcursionTypes = new List<ExcursionTypeViewModel>();
            this.Languages = new List<LanguageViewModel>();
        }


        public ICollection<ExcursionTypeViewModel> ExcursionTypes { get; set; }

        public ICollection<LanguageViewModel> Languages { get; set; }
    }
}
