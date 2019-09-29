

namespace Stopify.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Stopify.Data;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Services.Models;

    public class OrderService : IOrderService
    {
        private readonly StopifyDbContext context;

        public OrderService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateOrder(OrderServiceModel orderServiceModel)
        {

            Order order = orderServiceModel.To<Order>() ; //orderServiceModel.To<Order>()

            order.Status = await context.OrderStatuses
                .SingleOrDefaultAsync(orderStatus => orderStatus.Name == "Active");

            order.IssuedOn = DateTime.UtcNow;

            this.context.Orders.Add(order);
            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<OrderServiceModel> GetAllOrders()
        {
            return this.context.Orders.To<OrderServiceModel>();
        }
    }
}
