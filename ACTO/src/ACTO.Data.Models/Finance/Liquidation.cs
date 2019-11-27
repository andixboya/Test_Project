using ACTO.Data.Models.Excursions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.Models.Finance
{
    public class Liquidation : BaseModel<int>
    {
        //TODO: add this to tickets!
        public Liquidation()
        {
            this.Sales = new List<Sale>();
        }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        
        public string RepresentativeId { get; set; }
        public ACTOUser Representative { get; set; }
        public List<Sale> Sales { get; set; }

        //this does not concern the representative....
        public bool ReadyByCashier { get; set; }
        //according to this, you`ll search for a new liquidation...
        public bool ReadyByRepresentative { get; set; }
        public decimal TotalSum => this.Cash + this.CreditCard;
    }
}
