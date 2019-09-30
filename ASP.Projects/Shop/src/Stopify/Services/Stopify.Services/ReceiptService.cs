

namespace Stopify.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Stopify.Data;
    using Stopify.Data.Models;
    using Stopify.Services.Mapping;
    using Stopify.Services.Models;

    public class ReceiptService : IReceiptService
    {
        private readonly StopifyDbContext context;
        private readonly IOrderService orderService;

        public ReceiptService( StopifyDbContext context, IOrderService orderService)
        {
            this.context = context;
            this.orderService = orderService;
        }


        public async Task<string> CreateReceipt(string recipientId)
        {
            Receipt receipt = new Receipt()
            {
                IssuedOn = DateTime.UtcNow,
                RecipientId = recipientId
            };

            //here we load them
            await this.orderService.SetOrdersToReceipt(receipt);

            foreach (var order in receipt.Orders)
            {
                await this.orderService.CompleteOrder(order);
            }

            //when its added, we can use the id , as a redirect to our newly created receipt 
            await this.context.Receipts.AddAsync(receipt);
            int result =  await this.context.SaveChangesAsync();

            return receipt.Id;

        }

        public IQueryable<ReceiptServiceModel> GetAll()
        {
            return this.context.Receipts.To<ReceiptServiceModel>();
        }

        public IQueryable<ReceiptServiceModel> GetAllByRecipientId(string recipientId)
        {
            throw new NotImplementedException();
        }
    }
}
