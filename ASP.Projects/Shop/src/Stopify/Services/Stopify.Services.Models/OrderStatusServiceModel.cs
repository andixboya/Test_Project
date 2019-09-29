

namespace Stopify.Services.Models
{
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderStatusServiceModel : IMapFrom<OrderStatus>
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
