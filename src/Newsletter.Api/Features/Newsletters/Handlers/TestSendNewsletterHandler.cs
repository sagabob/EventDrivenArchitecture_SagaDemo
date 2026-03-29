using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class TestSendNewsletterHandler(NewsletterDbContext dbContext, IEmailService emailService)
    : IConsumer<TestSendNewsletter>
{
    public async Task Consume(ConsumeContext<TestSendNewsletter> context)
    {
        try
        {
            var msgId = context.Message.MessageId;
            var email = context.Message.Email;
            var ct = context.CancellationToken;

            await emailService.SendTestEmailAsync(context.Message.Email);

            var record = await dbContext.TestSendingMessages.FirstOrDefaultAsync(r => r.Id == msgId, ct);

            if (record is not null)
            {
                record.ReceivedEmail = email;
                record.ReceiveOnUtc = DateTime.UtcNow;

                // No explicit Update required when the entity is tracked, but harmless
                dbContext.TestSendingMessages.Update(record);

                Console.WriteLine($"Updated record for MessageId: {msgId}, Email: {email}");

                await dbContext.SaveChangesAsync(ct);

                Console.WriteLine($"Sending test newsletter to: {context.Message.Email}");
            }
            else
            {
                Console.WriteLine($"Sending test newsletter to: {context.Message.Email} is not received");

                //TODO
                //Notify Admin
            }
        }
        catch (Exception e)
        {
            // Log the exception
            Console.WriteLine($"Error processing TestSendNewsletter: {e.Message}");
            //TODO
        }
    }
}