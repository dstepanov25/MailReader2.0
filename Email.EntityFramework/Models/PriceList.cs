namespace Email.EntityFramework.Models {

    public class PriceList {
        // Primary key 
        public int Id { get; set; }

        // Foreign key 
        public int SupplierId { get; set; }

        public string PriceListName { get; set; }

        public string PriceListFileName { get; set; }

        public string PriceListExtension { get; set; }

        public System.DateTime? LastUpdate { get; set; }

        public bool UpdateFromMail { get; set; }

        public string PriceListUrl { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int MaxDaysForUpdate { get; set; }

        // Navigation properties 
        public virtual Supplier Supplier { get; set; }
    }

}