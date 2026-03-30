using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class ClearTestMessagesEndpoint(NewsletterDbContext dbContext)
    : Endpoint<EmptyRequest, ClearedRowsResponse>
{
    public override void Configure()
    {
        Delete("/api/newsletters/test-messages");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var removed = await dbContext.TestSendingMessages.ExecuteDeleteAsync(ct);
        await Send.OkAsync(new ClearedRowsResponse(removed), ct);
    }
}

public record ClearedRowsResponse(int Removed);