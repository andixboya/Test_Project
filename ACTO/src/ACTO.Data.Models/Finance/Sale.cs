

namespace ACTO.Data.Models.Finance
{
    using ACTO.Data.Models.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Sale : BaseModel<int>
    {
        public Sale()
        {
            this.Tickets = new List<Ticket>();
        }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }

        public int LiqudationId { get; set; }
        
        
        public Liquidation Liquidation { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public decimal TotalPrice { get; set; }

        //if there is at least one sale pending, all new tickets will go to it
        //if there are none, a new one is created 
        public bool IsFinalized { get; set; }
    }
}
