using RazorLight;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public interface IEmailService
{
    Task<bool> SendAsync(string toEmail, string subject, object model, string templateName);
}

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IRazorLightEngine _engine;

    public EmailService(ILogger<EmailService> logger, IRazorLightEngine engine)
    {
        _logger = logger;
        _engine = engine;
    }

    public async Task<bool> SendAsync(string to, string subject, object model, string templateName)
    {
        try
        {
            string body = await _engine.CompileRenderAsync($"{templateName}.cshtml", model);

            var message = new MimeMessage();
            var fromAddress = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "aswinalfarizi04@gmail.com";
            message.From.Add(MailboxAddress.Parse(fromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            var host = Environment.GetEnvironmentVariable("SMTP_HOST")!;
            if (string.IsNullOrEmpty(host))
            {
                _logger.LogError("SMTP_HOST is not defined in Environment Variables!");
                return false;
            }
            var port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            var user = Environment.GetEnvironmentVariable("SMTP_USER");
            var pass = Environment.GetEnvironmentVariable("SMTP_PASS");

            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(user, pass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email {Template} terkirim ke {To}", templateName, to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal kirim email {Template} ke {To}", templateName, to);
            return false;
        }
    }
}