

namespace ACTO.Services.Others
{
    using ACTO.Data;
    using ACTO.Data.Models.Finance;
    using ACTO.Web.ViewModels.Customers;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class CustomerServices : ICustomerServices
    {
        private readonly ACTODbContext context;

        public CustomerServices(ACTODbContext context)
        {
            this.context = context;
        }

        public async Task<Customer> CustomerCreate(CustomerViewModel model)
        {
            var newCustomer = new Customer()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            await context.Customers.AddAsync(newCustomer);

            return newCustomer;
        }
    }
}
