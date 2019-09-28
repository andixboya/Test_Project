namespace Stopify.Services.Models
{
    using AutoMapper;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Web.InputModels;
    using Stopify.Web.ViewModels;
    using System;

    public class ProductServiceModel : IMapFrom<Product>,IMapTo<Product> 
        ,IMapFrom<ProductCreateInputModel> , IMapTo<ProductDetailsViewModel>
        , IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public DateTime ManufacturedOn { get; set; }


        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductServiceModel>()
                .ForMember(destination => destination.ProductType,
                            opts => opts.MapFrom(origin => new ProductTypeServiceModel { Name = origin.ProductType.Name }));

            configuration.CreateMap<ProductCreateInputModel, ProductServiceModel>()
                .ForMember(destination => destination.ProductType,
                opts => opts.MapFrom(origin => new ProductTypeServiceModel { Name = origin.ProductType}));

            configuration.CreateMap<ProductServiceModel, ProductDetailsViewModel>()
                .ForMember(destination => destination.ProductType,
                opts => opts.MapFrom(origin => origin.ProductType.Name ));
        }
    }
}
