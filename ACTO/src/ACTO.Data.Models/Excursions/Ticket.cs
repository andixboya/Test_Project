

namespace ACTO.Data.Models.Excursions
{
    using ACTO.Data.Models.Finance;
    using System.Collections.Generic;

    public class Ticket : BaseModel<int>
    {
        //additional information about excursion reports.
        //TODO: potentially, i can make a sale instead of ticket, but ... i think this will be ok? 
        public int AdultCount { get; set; }
        public int ChildrenCount { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        //within the excursion will be the info about it...
        //prices will be taken from the excursion? 
        public int ExcursionId { get; set; }
        public Excursion Excursion { get; set; }
        public string Notes { get; set; }
        public int TouristCount => this.ChildrenCount + this.AdultCount;
        public int TourLanguageId { get; set; }
        public Language TourLanguage { get; set; }
        //probably... this only as id, the ticket won`t be needing anything from the sale.
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public bool IsDeleted { get; set; }
    }
}
