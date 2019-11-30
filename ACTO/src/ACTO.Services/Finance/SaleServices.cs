
namespace ACTO.Services.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ACTO.Data;
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;

    public class SaleServices : ISaleServices
    {
        private readonly ACTODbContext context;
        private readonly ILiquidationServices liquidationServices;
        public SaleServices(ACTODbContext context, ILiquidationServices liquidationServices)
        {
            this.context = context;
            this.liquidationServices = liquidationServices;
        }

        public async Task<Sale> GetOrCreateSaleById(string userId)
        {
            var sale = await context.
                Sales
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.IsFinalized == false && s.Liquidation.RepresentativeId == userId);

            //if no such sale is available we have to initialize it and  both liquidation if its not there!
            if (sale is null)
            {
                sale = new Sale();

                var liquidation = await this
                    .liquidationServices
                    .GetOrCreateById(userId);


                liquidation.Sales.Add(sale);
                sale.Liquidation = liquidation;

                await context.Sales.AddAsync(sale);
            }

            return sale;
        }
    }
}
