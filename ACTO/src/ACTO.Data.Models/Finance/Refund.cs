using System;
using System.Collections.Generic;
using System.Text;

namespace ACTO.Data.Models.Finance
{
    public class Refund : BaseModel<int>
    {
        //either amount or total.
        //if they have mixed... THEN SCREW IT!
        public decimal Amount => this.Cash + this.CreditCard;
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public DateTime Date { get; set; }
        //the below 2 will be necessary too!        
        public int ChildCount { get; set; }
        public int AdultCount { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
