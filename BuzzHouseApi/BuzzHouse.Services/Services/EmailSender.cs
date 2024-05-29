using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BuzzHouse.Services.Services;

public class EmailSender : IEmailSender
{
    private readonly SendgridOptions _sendgridOptions;

    public EmailSender(IOptions<SendgridOptions> sendgridOptions)
    {
        _sendgridOptions = sendgridOptions.Value;
    }
    public async Task<bool> SendEmailAsync(string to, string subject, string htmlMessage)
    {
        var senderAddress = new EmailAddress()
            { Name = "Buzz House Shop", Email = "buzzhouse-no-reply@adelinchis.ro" };
        var msg = GetMessage(senderAddress, subject, htmlMessage);
        msg.AddTo(new EmailAddress(to));
        var client = new SendGridClient(_sendgridOptions.ApiKey);
        var response = await client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }

    private static SendGridMessage GetMessage(EmailAddress emailAddress, string subject, string message)
    {
        return new SendGridMessage
        {
            From = emailAddress,
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
    }
}