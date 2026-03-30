using FastEndpoints;
using Newsletter.Api.Features.Newsletters.Messages;
using Newsletter.Api.Features.Newsletters.Services;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class SubscribeNewsletterEndpoint(ISubscribeNewsletterService subscribeNewsletterService)
    : Endpoint<SubscribeToNewsletter, string>
{
    public override void Configure()
    {
        Post("/api/newsletters/subscribe");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SubscribeToNewsletter req, CancellationToken ct)
    {
        var subscriber = await subscribeNewsletterService.SubscribeAsync(req.Email, ct);
        await Send.OkAsync($"Subscription request accepted. Id: {subscriber.Id}", ct);
    }
}