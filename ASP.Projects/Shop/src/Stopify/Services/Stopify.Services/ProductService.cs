using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stopify.Data;
using Stopify.Data.Models;
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
            var productType = this.context.ProductTypes.FirstOrDefault(p => p.Name == productServiceModel.ProductType.Name);

            Product product = new Product
            {
                Name = productServiceModel.Name,
                Price = productServiceModel.Price,
                ManufacturedOn = productServiceModel.ManufacturedOn,
                ProductType=productType,
                Picture=productServiceModel.Picture
                
            };

            context.Products.Add(product);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            ProductType productType = new ProductType
            {
                Name = productTypeServiceModel.Name
            };

            context.ProductTypes.Add(productType);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductServiceModel> GetAllProducts()
        {
            return this.context.Products.Select(p => new ProductServiceModel()
            {
                Name = p.Name,
                Id = p.Id,
                ManufacturedOn = p.ManufacturedOn,
                Picture = p.Picture,
                Price = p.Price,
                ProductType = new ProductTypeServiceModel()
                {
                    Id = p.ProductType.Id,
                    Name = p.ProductType.Name
                },
                ProductTypeId = p.ProductType.Id
            });
        }

        public  IQueryable<ProductTypeServiceModel> GetAllProductTypes()
        {
            return this.context.ProductTypes
                .Select(productType => new ProductTypeServiceModel
                {
                    Id = productType.Id,
                    Name = productType.Name
                });
        }

    }
}
