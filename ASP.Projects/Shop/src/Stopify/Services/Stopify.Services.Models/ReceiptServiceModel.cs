

namespace Stopify.Services.Models
{
    using AutoMapper;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Web.ViewModels.Receipt.Details;
    using Stopify.Web.ViewModels.Receipt.Profile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class ReceiptServiceModel :IMapFrom<Receipt> , IMapTo<ReceiptDetailsViewModel> , IMapTo<ReceiptProfileViewModel>
        ,IHaveCustomMappings
    {

        public ReceiptServiceModel()
        {
            this.Orders = new List<OrderServiceModel>();
        }
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public List<OrderServiceModel> Orders { get; set; }

        public string RecipientId { get; set; }

        public StopifyUserServiceModel Recipient { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ReceiptServiceModel, ReceiptDetailsViewModel>()
                            .ForMember(destination => destination.Recipient,
                                        opts => opts.MapFrom(origin => origin.Recipient.UserName));

            configuration.CreateMap<ReceiptServiceModel, ReceiptProfileViewModel>()
                            .ForMember(destination => destination.Total,
                                        opts => opts.MapFrom(origin => origin.Orders.Sum(o => o.Quantity * o.Product.Price)))
                            .ForMember(destination => destination.Products,
                            opts => opts.MapFrom(origin => origin.Orders.Sum(o=> o.Quantity)));

        }
    }
}
