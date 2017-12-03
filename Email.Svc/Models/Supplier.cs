namespace Email.Svc.Models {

    public class Supplier {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Emails { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Contacts { get; set; }

        public bool IsNew { get; set; }
    }

}