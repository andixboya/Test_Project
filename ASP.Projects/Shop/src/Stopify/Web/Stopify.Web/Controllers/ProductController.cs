

namespace Stopify.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Stopify.Data;
    using Stopify.Services;
    using Stopify.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Details(string id)
        {
            //TODO: make getProdubyById work async!
            var productInfo = this.productService.GetProductById(id);
            
            var product = Mapper.Map<ProductDetailsViewModel>(productInfo);

            return this.View(product);
        }
    }
}
