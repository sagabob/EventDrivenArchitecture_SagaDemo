using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Features.Newsletters.Sagas;

namespace Newsletter.Api.Databases;

public class NewsletterDbContext(DbContextOptions<NewsletterDbContext> options) : DbContext(options)
{
    public DbSet<TestSendingMessage> TestSendingMessages { get; set; }

    public DbSet<Subscriber> Subscribers { get; set; }

    public DbSet<NewsletterOnboardingSagaData> SagaData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.Property(e => e.OnboardingStatus)
                .HasConversion<string>()
                .HasMaxLength(32);
            entity.Property(e => e.OnboardingFaultReason).HasMaxLength(2000);
        });

        modelBuilder.ApplyConfiguration(new NewsletterOnboardingSagaMap());

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}