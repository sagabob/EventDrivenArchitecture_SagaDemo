using MassTransit;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SubscribeToNewsletterHandler(NewsletterDbContext dbContext) : IConsumer<SubscribeToNewsletter>
{
    public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
    {
        var subscriber = dbContext.Subscribers.Add(new Subscriber
        {
            Id = Guid.NewGuid(),
            Email = context.Message.Email,
            SubscribedOnUtc = DateTime.UtcNow,
            OnboardingStatus = SubscriberOnboardingStatus.Pending
        });

        await context.Publish(new SubscriberCreated
        {
            SubscriberId = subscriber.Entity.Id,
            Email = context.Message.Email
        });

        await dbContext.SaveChangesAsync();
    }
}