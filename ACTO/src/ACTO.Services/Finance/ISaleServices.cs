

namespace ACTO.Services.Finance
{
    using ACTO.Data.Models.Finance;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISaleServices
    {

        Task<Sale> GetOrCreateSaleById(string userId);


    }
}
