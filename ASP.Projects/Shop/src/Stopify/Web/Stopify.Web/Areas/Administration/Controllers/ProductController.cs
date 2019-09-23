

namespace Stopify.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Stopify.Web.InputModels;
    using Stopify.Services;
    using Stopify.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Stopify.Web.ViewModels;

    public class ProductController : AdminController
    {

        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Administration/Product/Create")]
        public async Task<IActionResult> Create()
        {
            var allProductTypes = await this.productService.GetAllProductTypes();

            //note: this is important too, check it! 
            this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
            {
                Name = productType.Name
            }).ToList();


            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInputModel inputModel)
        {
            //note: will be fixed with automapper!
            ProductServiceModel serviceModel = new ProductServiceModel()
            {
                ManufacturedOn = inputModel.ManufacturedOn,
                Name = inputModel.Name,
                //Picture=inputModel.Picture,
                Price = inputModel.Price,
                ProductType = new ProductTypeServiceModel()
                {
                    Name = inputModel.ProductType
                }
            };

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
            ProductTypeServiceModel productTypeServiceModel = new ProductTypeServiceModel
            {
                Name = productTypeCreateInputModel.Name
            };

            await this.productService.CreateProductType(productTypeServiceModel);

            return this.Redirect("/");
        }




       


    }
}
