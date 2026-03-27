using Microsoft.EntityFrameworkCore;

namespace Newsletter.Api.Databases
{
    public class NewsletterDbContext(DbContextOptions<NewsletterDbContext> options) : DbContext(options)
    {
        public DbSet<TestSendingMessage> TestSendingMessages { get; set; }
    }

}
