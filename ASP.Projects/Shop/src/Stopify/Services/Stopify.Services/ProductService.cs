using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Services
{
    public class ProductService : IProductService
    {
        private readonly StopifyDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public ProductService(StopifyDbContext context, ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> Create(ProductServiceModel productServiceModel)
        {
            //works
            var productType = this.context.ProductTypes.FirstOrDefault(p => p.Name == productServiceModel.ProductType.Name);

            Product product = Mapper.Map<Product>(productServiceModel);
            product.ProductType = productType;
            #region old mapping    
            //    new Product
            //{
            //    Name = productServiceModel.Name,
            //    Price = productServiceModel.Price,
            //    ManufacturedOn = productServiceModel.ManufacturedOn,
            //    ProductType=productType,
            //    Picture=productServiceModel.Picture

            //};
            #endregion

            context.Products.Add(product);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            ProductType productType = Mapper.Map<ProductType>(productTypeServiceModel);
            #region old mapping
            //    new ProductType
            //{
            //    Name = productTypeServiceModel.Name
            //};
            #endregion

            context.ProductTypes.Add(productType);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductServiceModel> GetAllProducts()
        {

            var result= this.context.Products.To<ProductServiceModel>();
            
            return result;
            #region old mapping
            //    .Select(p => new ProductServiceModel()
            //{
            //    Name = p.Name,
            //    Id = p.Id,
            //    ManufacturedOn = p.ManufacturedOn,
            //    Picture = p.Picture,
            //    Price = p.Price,
            //    ProductType = new ProductTypeServiceModel()
            //    {
            //        Id = p.ProductType.Id,
            //        Name = p.ProductType.Name
            //    },
            //    ProductTypeId = p.ProductType.Id
            //});
            #endregion
        }

        public  IQueryable<ProductTypeServiceModel> GetAllProductTypes()
        {
            
            var result= this.context.ProductTypes.To<ProductTypeServiceModel>();
            
            return result;
            #region old mapping
            //.Select(productType => new ProductTypeServiceModel
            //{
            //    Id = productType.Id,
            //    Name = productType.Name
            //});
            #endregion
            //from product

        }

    }
}
