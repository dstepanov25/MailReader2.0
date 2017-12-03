using Microsoft.EntityFrameworkCore;

namespace Email.EntityFramework.Models {

    public class EmailContext : DbContext {
        public EmailContext(DbContextOptions options) : base(options) {
        }

        public DbSet<Supplier> Suppliers { get; set; }
    }

}