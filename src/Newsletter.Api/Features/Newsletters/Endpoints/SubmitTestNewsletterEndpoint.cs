using FastEndpoints;
using MassTransit;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class SubmitTestNewsletterEndpoint(IPublishEndpoint publisher, NewsletterDbContext dbContext)
    : Endpoint<SubscribeToNewsletter, TrackIdResponse>
{
    public override void Configure()
    {
        Post("/api/newsletters/test");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SubscribeToNewsletter req, CancellationToken ct)
    {
        var testRecord = dbContext.TestSendingMessages.Add(new TestSendingMessage
        {
            Id = Guid.NewGuid(),
            SendingEmail = req.Email,
            ReceivedEmail = null,
            SentOnUtc = DateTime.UtcNow,
            ReceiveOnUtc = null
        });

        await dbContext.SaveChangesAsync(ct);

        await publisher.Publish(new TestSendNewsletter(testRecord.Entity.Id, req.Email), ct);

        var response = new TrackIdResponse(testRecord.Entity.Id);
        await Send.OkAsync(response, ct);
    }
}