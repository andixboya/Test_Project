

namespace Stopify.Services.Models
{
    using AutoMapper;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Web.ViewModels.Receipt.Details;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ReceiptServiceModel :IMapFrom<Receipt> , IMapTo<ReceiptDetailsViewModel> 
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
        }
    }
}
