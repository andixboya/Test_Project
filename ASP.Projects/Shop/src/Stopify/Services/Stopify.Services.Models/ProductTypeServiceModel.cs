

namespace Stopify.Services.Models
{
    using AutoMapper;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Web.InputModels;
    using Stopify.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductTypeServiceModel : IMapFrom<ProductType>, IMapFrom<ProductCreateInputModel>,
        IMapTo<ProductType>, IMapTo<ProductCreateProductTypeViewModel> 

    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            



        }
    }
}
