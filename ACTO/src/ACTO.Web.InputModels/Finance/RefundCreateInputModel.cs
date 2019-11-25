namespace ACTO.Web.InputModels.Finance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    public class RefundCreateInputModel
    {
        public int MaxChildrenCount { get; set; }

        public int MaxAdultCount { get; set; }

        [Display(Name = "Ad.")]
        public int AdultToRefund { get; set; }
        [Display(Name = "Ch.")]
        public int ChildrenToRefund { get; set; }


        [Display(Name = "Price per adult")]
        public decimal PricePerAdult { get; set; }

        [Display(Name = "Price per child")]
        public decimal PricePerChild { get; set; }

        public int TicketId { get; set; }

        [Display(Name ="Sum to refund")]
        public decimal SumToRefund { get; set; }


    }
}
