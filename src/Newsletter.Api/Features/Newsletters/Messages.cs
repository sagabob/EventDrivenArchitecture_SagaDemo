namespace Newsletter.Api.Features.Newsletters
{
    public record SubscribeToNewsletter(string Email);

    public record TrackIdResponse(string TrackId);
}
