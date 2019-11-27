

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
        public Representative()
        {
            this.Liquidations = new List<Liquidation>();
        }
        
        public string UserId { get; set; }
        public ACTOUser User { get; set; }
        
        public ICollection<Liquidation> Liquidations { get; set; }

        public bool IsDeleted { get; set; }
    }
}
