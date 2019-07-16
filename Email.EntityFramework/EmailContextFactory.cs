using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Email.EntityFramework {

    public class EmailContextFactory : IDesignTimeDbContextFactory<EmailContext> {
        public EmailContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<EmailContext>();
            optionsBuilder.UseSqlite("Data Source=blog.db");

            return new EmailContext(optionsBuilder.Options);
        }
    }

}