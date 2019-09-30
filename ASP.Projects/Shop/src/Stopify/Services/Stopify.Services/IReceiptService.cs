

namespace Stopify.Services
{
    using Stopify.Data.Models;
    using Stopify.Services.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReceiptService
    {

        Task<string> CreateReceipt(string recipientId);

        IQueryable<ReceiptServiceModel> GetAll();

        IQueryable<ReceiptServiceModel> GetAllByRecipientId(string recipientId);

    }
}
