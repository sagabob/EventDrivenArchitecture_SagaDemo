using FastEndpoints;

namespace Newsletter.Api.Features.Newsletters
{
    public class NewsletterGetEndpoint : Endpoint<EmptyRequest, string>
    {
        public override void Configure()
        {
            Get("/newsletter/info");
            AllowAnonymous();
        }

        public override Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            // This endpoint could return some general information about the newsletter,
            // such as the frequency of emails, topics covered, etc.
            return Send.OkAsync("Newsletter information", ct);
        }
    }
}
