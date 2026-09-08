using MailKit.Net.Smtp;
using MimeKit;

namespace exam_system.Common.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Examination System", _configuration["Smtp:SenderEmail"] ?? "no-reply@examsystem.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _configuration["Smtp:Host"] ?? "localhost",
            int.Parse(_configuration["Smtp:Port"] ?? "25"),
            MailKit.Security.SecureSocketOptions.Auto);

        if (!string.IsNullOrEmpty(_configuration["Smtp:Username"]))
        {
            await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
        }

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
