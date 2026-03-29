using FastEndpoints;
using MassTransit;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class SubscribeNewsletterEndpoint(IPublishEndpoint publisher, NewsletterDbContext dbContext)
    : Endpoint<SubscribeToNewsletter, string>
{
    public override void Configure()
    {
        Post("/api/newsletters/subscribe");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SubscribeToNewsletter req, CancellationToken ct)
    {
        // Bus outbox: Publish stages the message; SaveChanges commits OutboxMessage so it can reach RabbitMQ.
        await publisher.Publish(req, ct);
        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync("Subscription request accepted.", ct);
    }
}