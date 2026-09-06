using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MCV_Mini_Project.Options;
using MCV_Mini_Project.Services.Interface;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MCV_Mini_Project.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtp;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpOptions> smtp, ILogger<EmailService> logger)
        {
            _smtp = smtp.Value;
            _logger = logger;
        }

        public bool IsConfigured => _smtp.IsConfigured;

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            if (!IsConfigured)
                throw new InvalidOperationException("SMTP is not configured. Set SMTP_USER and SMTP_PASSWORD in the .env file.");

            var from = string.IsNullOrWhiteSpace(_smtp.From) ? _smtp.User : _smtp.From;
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(from));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            foreach (var address in ResolveRecipients(to))
                message.To.Add(MailboxAddress.Parse(address));

            using var client = new SmtpClient();
            var secure = _smtp.Port == 465
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await client.ConnectAsync(_smtp.Host, _smtp.Port, secure);
            await client.AuthenticateAsync(_smtp.User, _smtp.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailConfirmationAsync(string to, string confirmationLink)
        {
            var html = $"""
                <div style="font-family:Montserrat,Arial,sans-serif;max-width:560px;margin:0 auto;padding:24px;background:#f2f1f8;color:#2c2b31">
                  <div style="background:#fff;border-radius:10px;padding:32px;border-top:4px solid #ff8a00">
                    <h2 style="margin-top:0;color:#2c2b31">Confirm your email</h2>
                    <p>Thanks for registering with eLearn. Please confirm your email address by clicking the button below. This link expires and can be used only once.</p>
                    <p style="text-align:center;margin:32px 0">
                      <a href="{WebUtility.HtmlEncode(confirmationLink)}"
                         style="background:#ff8a00;color:#fff;text-decoration:none;padding:12px 24px;border-radius:6px;font-weight:700;display:inline-block">
                        Confirm email
                      </a>
                    </p>
                    <p style="font-size:13px;color:#6c6a74">If the button does not work, copy and paste this URL into your browser:</p>
                    <p style="font-size:12px;word-break:break-all;color:#44425a">{WebUtility.HtmlEncode(confirmationLink)}</p>
                  </div>
                </div>
                """;

            await SendAsync(to, "Confirm your eLearn account", html);
            _logger.LogInformation("Confirmation email sent.");
        }

        private IEnumerable<string> ResolveRecipients(string to)
        {
            var recipients = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(to))
                recipients.Add(to.Trim());

            if (!string.IsNullOrWhiteSpace(_smtp.ConfirmationTo))
                recipients.Add(_smtp.ConfirmationTo.Trim());

            return recipients;
        }
    }
}
