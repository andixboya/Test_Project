

namespace ACTO.Services.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ACTO.Data;
    using ACTO.Data.Models.Finance;
    using Microsoft.EntityFrameworkCore;

    public class LiquidationServices : ILiquidationServices
    {
        private readonly ACTODbContext context;

        public LiquidationServices( ACTODbContext context)
        {
            this.context = context;
        }

        public async Task<Liquidation> GetOrCreateById(string userId)
        {
            var liquidation= await context 
                    .Liquidations
                    .FirstOrDefaultAsync(l => l.ReadyByRepresentative == false
                    && l.RepresentativeId == userId);


            if (liquidation is null)
            {
                liquidation = new Liquidation()
                {
                    RepresentativeId = userId
                };
                await context.Liquidations.AddAsync(liquidation);
            }

            return liquidation;
        }
    }
}
