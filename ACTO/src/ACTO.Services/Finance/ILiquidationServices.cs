

namespace ACTO.Services.Finance
{
    using ACTO.Data.Models.Finance;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILiquidationServices
    {

        Task<Liquidation> GetOrCreateById(string userId);
    }
}
