using ACTO.Data.Models.Excursions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.Models.Finance
{
    public class Liquidation : BaseModel<int>
    {
        //TODO: add this to tickets!
       
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; }
        public decimal TotalSum => this.Cash + this.CreditCard;
        public ICollection<Sale> ReportedSales { get; set; }
    }
}
