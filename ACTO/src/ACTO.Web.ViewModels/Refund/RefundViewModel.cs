using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACTO.Web.ViewModels.Refund
{
    public class RefundViewModel
    {

        //they  won`t be displayed, instead they`ll be in a table!
        public decimal Amount => this.Cash + this.CreditCard;
        
        public decimal Cash { get; set; }
        
        public decimal CreditCard { get; set; }
        
        //i`ll skip this one i guess...
        //public DateTime Date { get; set; }
        //the below 2 will be necessary too!        
        public int ChildCount { get; set; }
        public int AdultCount { get; set; }
    }
}
