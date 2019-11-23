

namespace ACTO.Data.Models.Finance
{
    using ACTO.Data.Models.Excursions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Customer : BaseModel<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ICollection<Ticket> Tickets { get; set; }
        
        //eventually if we need the id of the ticket we can put it..., but i think it won`t be necessary
        public int TicketId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
