

namespace Stopify.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Stopify.Services;
    using Stopify.Services.Models;
    using Stopify.Web.InputModels;
    using Stopify.Web.ViewModels;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IOrderService orderService;

        public ProductController(IProductService productService , IOrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
        }

        public IActionResult Details(string id)
        {
            //TODO: make getProdubyById work async!
            var productInfo = this.productService.GetProductById(id);

            var product = Mapper.Map<ProductDetailsViewModel>(productInfo);

            return this.View(product);
        }


        public async Task<IActionResult> Order(ProductOrderInputModel inputModel)
        {
            OrderServiceModel orderServiceModel = Mapper.Map<OrderServiceModel>(inputModel);

            //orderServiceModel.IssuerId = this.User.Identity.Name;
            //note: това търси този, който го е създал, а не този, който купува!!!! ????? 
            
            orderServiceModel.IssuerId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
             await this.orderService.CreateOrder(orderServiceModel);
            

            return this.Redirect("/Home/Index");
        }
    }
}
