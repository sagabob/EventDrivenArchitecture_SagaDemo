using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class ListTestMessagesEndpoint(NewsletterDbContext dbContext) : Endpoint<EmptyRequest, List<TestSendingMessage>>
{
    public override void Configure()
    {
        Get("/api/newsletters/test-messages");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var messages = await dbContext.TestSendingMessages
            .OrderByDescending(m => m.SentOnUtc)
            .Take(50)
            .ToListAsync(ct);

        await Send.OkAsync(messages, ct);
    }
}