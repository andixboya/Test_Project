

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

        //if its emtpy, this means its not yet sold


        //this is important!
        //wonder how i`ll... refund these...
        //will choose between tickets... and then between the count of adult/children ( within the excursion will be the price)
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; }

        //will take the customer from the tickets!
        //we need the tickets, because there can be multiple tickets to one sale!
        //children/adult count will be subtracted from the tickets count!
        //after they are removed... we`ll subtract the sums from the cash/credit cards
        public ICollection<Ticket> Tickets { get; set; }
        //not sure if i`ll be needing the below
        public decimal TotalPrice { get; set; }
    }
}
