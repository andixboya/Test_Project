

namespace Stopify.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Stopify.Web.InputModels;
    using Stopify.Services;
    using Stopify.Services.Models;
    using System;
    using System.Collections.Generic;
    //using System.Linq;
    using System.Threading.Tasks;
    using Stopify.Web.ViewModels;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    public class ProductController : AdminController
    {

        private readonly IProductService productService;
        private readonly ICloudinaryService cloudinaryService;

        public ProductController(IProductService productService,ICloudinaryService cloudinaryService)
        {
            this.productService = productService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet("/Administration/Product/Create")]
        public async Task<IActionResult> Create()
        {
            var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

            //note: this is important too, check it! 
            this.ViewData["types"] = allProductTypes
                .Select(productType => Mapper.Map<ProductCreateProductTypeViewModel>(productType))
                .ToList();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                List<ProductTypeServiceModel> allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

                this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
                {
                    Name = productType.Name
                })
                    .ToList();

                return this.View(inputModel);
            }



            var imageUrl = await cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);
            //var secondImage = await cloudinaryService.UploadPictureAsync(inputModel.Picture);

            ProductServiceModel serviceModel = Mapper.Map<ProductServiceModel>(inputModel);
            serviceModel.Picture = imageUrl;

            #region old mapping
            //    new ProductServiceModel()
            //{
            //    ManufacturedOn = inputModel.ManufacturedOn,
            //    Name = inputModel.Name,
            //    //Picture=inputModel.Picture,
            //    Price = inputModel.Price,
            //    ProductType = new ProductTypeServiceModel()
            //    {
            //        Name = inputModel.ProductType
            //    },
            //    Picture = imageUrl
            //};
            #endregion


            var isCreated = await productService.Create(serviceModel);

            return this.Redirect("/Home/Index");
        }


        [HttpGet("/Administration/Product/Type/Create")]
        public async Task<IActionResult> CreateType()
        {
            return this.View("Type/Create");
        }

        [HttpPost("/Administration/Product/Type/Create")]
        public async Task<IActionResult> CreateType(ProductTypeCreateInputModel productTypeCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(productTypeCreateInputModel);
            }

            //for one it is not worth it? 
            ProductTypeServiceModel productTypeServiceModel = new ProductTypeServiceModel
            {
                Name = productTypeCreateInputModel.Name
            };

            await this.productService.CreateProductType(productTypeServiceModel);

            return this.Redirect("/");
        }

    }
}
