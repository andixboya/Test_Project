

namespace Stopify.Services
{
    using Stopify.Data.Models;
    using Stopify.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<bool> CreateOrder(OrderServiceModel orderServiceModel);

        IQueryable<OrderServiceModel> GetAllOrders();

        //this will be used by the receipt service to tell the 
        //oderService that he needs to change all the orders to complete
        Task SetOrdersToReceipt(Receipt receipt);


        //this i think can easily be private? 
        Task<bool> CompleteOrder(Order order);

    }
}
