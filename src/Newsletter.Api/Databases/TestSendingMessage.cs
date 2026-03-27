namespace Newsletter.Api.Databases;

public class TestSendingMessage
{
    public Guid Id { get; set; }
    public string SendingEmail { get; set; }

    public string? ReceivedEmail { get; set; }
    public DateTime SentOnUtc { get; set; }

    public DateTime? ReceiveOnUtc { get; set; }
}