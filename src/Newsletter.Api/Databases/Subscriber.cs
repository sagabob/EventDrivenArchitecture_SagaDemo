namespace Newsletter.Api.Databases;

public class Subscriber
{
    public Guid Id { get; set; }

    public string Email { get; set; } 

    public DateTime SubscribedOnUtc { get; set; }
}