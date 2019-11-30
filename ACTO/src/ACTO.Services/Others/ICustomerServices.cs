

namespace ACTO.Services.Others
{
    using ACTO.Data.Models.Finance;
    using ACTO.Web.ViewModels.Customers;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICustomerServices
    {
        Task<Customer> CustomerCreate(CustomerViewModel customer);
    }
}
