

namespace Stopify.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Stopify.Services;
    using Stopify.Services.Mapping;
    using Stopify.Web.ViewModels.Order;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class OrderController : Controller
    {

        private readonly IOrderService orderService;
        

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
            
            
        }

        [HttpGet(Name = "Cart")]
        public async  Task<IActionResult> Cart()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<OrderCartViewModel> orderCartViewModel = await this.orderService.GetAllOrders()
                .Where(o=> o.IssuerId== userId)
                .Select(order=> order.To<OrderCartViewModel>())
                .ToListAsync();

            return this.View(orderCartViewModel);
        }

        [HttpPost(Name = "Cart")]
        public IActionResult Cart(string name)
        {

            return this.Ok();
        }
    }
}
