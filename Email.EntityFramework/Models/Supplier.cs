using System.Collections.Generic;

namespace Email.EntityFramework.Models {

    public class Supplier {
        // Primary key 
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string FolderName { get; set; }

        public string Emails { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Contacts { get; set; }

        public bool IsNew { get; set; }

        // Navigation property 
        public virtual ICollection<PriceList> PriceLists { get; set; }
    }

}