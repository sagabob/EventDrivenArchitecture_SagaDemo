using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Features.Newsletters.Sagas;

namespace Newsletter.Api.Databases;

public class NewsletterDbContext(DbContextOptions<NewsletterDbContext> options) : DbContext(options)
{
    public DbSet<TestSendingMessage> TestSendingMessages { get; set; }

    public DbSet<Subscriber> Subscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}