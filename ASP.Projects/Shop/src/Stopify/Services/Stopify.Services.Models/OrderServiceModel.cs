

namespace Stopify.Services.Models
{
    using AutoMapper;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Web.InputModels;
    using Stopify.Web.ViewModels.Order;
    using Stopify.Web.ViewModels.Receipt.Details;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    public class OrderServiceModel :IMapFrom<ProductOrderInputModel> , IMapTo<Order> , IMapFrom<Order>,
        IMapTo<OrderCartViewModel>  , IMapTo<ReceiptDetailsOrderViewModel> 
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public string ProductId { get; set; }

        
        public ProductServiceModel Product { get; set; }

        public int Quantity { get; set; }

        public string IssuerId { get; set; }

        public StopifyUserServiceModel Issuer { get; set; }

        public int StatusId { get; set; }

        public OrderStatusServiceModel Status { get; set; }

        
    }
}
