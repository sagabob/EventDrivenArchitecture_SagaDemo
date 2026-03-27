using FastEndpoints;

namespace Newsletter.Api.Features.Newsletters;

public class NewsletterPostEndpoint : Endpoint<SubscribeToNewsletter, TrackIdResponse>
{
    public override void Configure()
    {
        Post("/newsletter/subscribe");
        AllowAnonymous();
    }

    public override Task HandleAsync(SubscribeToNewsletter req, CancellationToken ct)
    {
        // Here you would typically add the email to your newsletter subscription list
        // and generate a tracking ID for the subscription.
        var trackId = Guid.NewGuid().ToString(); // Simulating track ID generation
        var response = new TrackIdResponse(trackId);
        return Send.OkAsync(response, ct);
    }
}