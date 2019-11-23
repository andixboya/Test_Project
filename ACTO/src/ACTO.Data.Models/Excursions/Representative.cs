

namespace ACTO.Data.Models.Excursions
{
    using ACTO.Data.Models.Finance;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Representative :BaseModel<int>
    {

        //this should be redundant? 
        //public int UserId { get; set; }
        
        public string UserId { get; set; }
        public ACTOUser User { get; set; }
        public ICollection<Ticket> SoldTickets { get; set; }

        public ICollection<Sale> Sales { get; set; }
        public ICollection<Liquidation> Liquidations { get; set; }

        public bool IsDeleted { get; set; }
    }
}
