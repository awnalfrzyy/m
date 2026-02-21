using System.Net;
using System.Net.Mail;
using RazorLight;

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

            using var smtp = new SmtpClient
            {
                Host = Environment.GetEnvironmentVariable("SMTP_HOST")!,
                Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587"),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("SMTP_USER"),
                    Environment.GetEnvironmentVariable("SMTP_PASS")
                )
            };

            var message = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_FROM")!, "Diggie Server"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await smtp.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal kirim email {Template}", templateName);
            return false;
        }
    }
}