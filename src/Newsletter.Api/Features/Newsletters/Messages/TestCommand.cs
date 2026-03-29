namespace Newsletter.Api.Features.Newsletters.Messages
{
    public record TrackIdResponse(Guid TrackId);

    public record TestSendNewsletter(Guid MessageId, string Email);
}
