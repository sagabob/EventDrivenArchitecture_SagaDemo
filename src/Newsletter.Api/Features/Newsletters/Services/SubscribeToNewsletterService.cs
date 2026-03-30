using MassTransit;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Services;

public class SubscribeToNewsletterService(IPublishEndpoint publisher, NewsletterDbContext dbContext)
    : ISubscribeNewsletterService
{
    public async Task<Subscriber> SubscribeAsync(string email, CancellationToken cancellationToken)
    {
        var subscriber = dbContext.Subscribers.Add(new Subscriber
        {
            Id = Guid.NewGuid(),
            Email = email,
            SubscribedOnUtc = DateTime.UtcNow,
            OnboardingStatus = SubscriberOnboardingStatus.Pending
        });

        // Bus outbox: Publish stages the message; SaveChanges commits OutboxMessage so it can reach RabbitMQ.
        await publisher.Publish(new SubscriberCreated { SubscriberId = subscriber.Entity.Id, Email = email },
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return subscriber.Entity;
    }
}