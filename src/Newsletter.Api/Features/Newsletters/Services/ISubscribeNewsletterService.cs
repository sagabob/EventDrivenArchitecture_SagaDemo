using Newsletter.Api.Databases;

namespace Newsletter.Api.Features.Newsletters.Services;

public interface ISubscribeNewsletterService
{
    Task<Subscriber> SubscribeAsync(string email, CancellationToken cancellationToken);
}