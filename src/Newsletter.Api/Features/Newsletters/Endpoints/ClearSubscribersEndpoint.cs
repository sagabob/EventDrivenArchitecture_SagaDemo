using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class ClearSubscribersEndpoint(NewsletterDbContext dbContext)
    : Endpoint<EmptyRequest, ClearedSubscribersResponse>
{
    public override void Configure()
    {
        Delete("/api/newsletters/subscribers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var sagaRemoved = await dbContext.SagaData.ExecuteDeleteAsync(ct);
        var subscribersRemoved = await dbContext.Subscribers.ExecuteDeleteAsync(ct);

        await Send.OkAsync(new ClearedSubscribersResponse(subscribersRemoved, sagaRemoved), ct);
    }
}

public record ClearedSubscribersResponse(int SubscribersRemoved, int SagaInstancesRemoved);