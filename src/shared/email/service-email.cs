using System.Net;
using System.Net.Mail;

namespace diggie_server.src.shared.email;

public class EmailService
{
    // private readonly IConfiguration _config;

    // public EmailService(IConfiguration config)
    // {
    //     _config = config;
    // }

    public async Task SendAsync(string to, string subject, string body)
    {
        var smtp = new SmtpClient
        {
            Host = Environment.GetEnvironmentVariable("SMTP_HOST")!,
            Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                Environment.GetEnvironmentVariable("SMTP_USER"),
                Environment.GetEnvironmentVariable("SMTP_PASS")
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_FROM")!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);
        await smtp.SendMailAsync(message);
    }
}