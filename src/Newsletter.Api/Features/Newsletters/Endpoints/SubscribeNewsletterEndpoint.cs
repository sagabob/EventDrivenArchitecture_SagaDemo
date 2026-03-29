using FastEndpoints;
using MassTransit;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class SubscribeNewsletterEndpoint(IPublishEndpoint publisher)
    : Endpoint<SubscribeToNewsletter, string>
{
    public override void Configure()
    {
        Post("/api/newsletters/subscribe");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SubscribeToNewsletter req, CancellationToken ct)
    {
        await publisher.Publish(req, ct);
        await Send.OkAsync("Subscription request accepted.", ct);
    }
}