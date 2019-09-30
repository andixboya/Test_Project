

namespace Stopify.Services
{
    using Microsoft.EntityFrameworkCore;
    using Stopify.Data;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Services.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly StopifyDbContext context;

        public OrderService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CompleteOrder(Order order)
        {
            //here i think we could have just added the order itself perhaps.. but in my opinion it could work with just an object
            //because we are repeating an action... 
            var completed = await this.context.OrderStatuses.SingleOrDefaultAsync(s => s.Name == "Completed");
            order.Status = completed;

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateOrder(OrderServiceModel orderServiceModel)
        {

            Order order = orderServiceModel.To<Order>(); //orderServiceModel.To<Order>()

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

        public async Task SetOrdersToReceipt(Receipt receipt)
        {
            var activeStatus = await this.context.OrderStatuses.SingleOrDefaultAsync(s => s.Name == "Active");

            var orders =  await this.context.Orders.Where(o => o.IssuerId == receipt.RecipientId && o.Status.Name == activeStatus.Name).ToListAsync();

            receipt.Orders = orders;
        }
    }
}
