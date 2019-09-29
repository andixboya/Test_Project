

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

    }
}
