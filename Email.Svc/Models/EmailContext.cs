using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Email.Svc.Models {

    public class EmailContext : DbContext {
        //public EmailContext(DbContextOptions options) : base(options)
        //{
        //}

        public DbSet<Supplier> Suppliers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        //{
        //    //optionbuilder.UseSqlite(@"Data Source=c:\Sample.db");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }
    }

}