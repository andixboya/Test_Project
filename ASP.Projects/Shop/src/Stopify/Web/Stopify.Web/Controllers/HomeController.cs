using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stopify.Services;
using Stopify.Web.Models;
using Stopify.Web.ViewModels.Home.Index;

namespace Stopify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IProductService productService;

        
        public HomeController(IProductService productService, ILogger<HomeController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                List<ProductHomeViewModel> products = await this.productService.GetAllProducts()
                    .Select(product => new ProductHomeViewModel
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Picture = product.Picture
                    })
                    .ToListAsync();

                return this.View(products);
            }

            return View();
        }

    }
}
