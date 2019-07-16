using Email.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Email.EntityFramework {

    public class EmailContext : DbContext {
        public EmailContext(DbContextOptions options) : base(options) {
        }

        //public EmailContext(DbContextOptions<EmailContext> options) : base(options)
        //{
        //}

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PriceList> PriceLists { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //    optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        //}
    }

}