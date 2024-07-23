using MimeKit;
using Microsoft.Extensions.Options;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;
using Infrastructure.Options;
using Application.Email;
using Application.Services;

namespace Infrastructure.Services;

public class EmailSenderServiceAsync(IOptions<EmailSettings> mailSettings) : IEmailSenderServiceAsync
{
    public async Task<string> SendAsync(EmailMetadata emailMetadata)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSettings.Value.Email),
                Subject = emailMetadata.Subject
            };

            email.To.Add(MailboxAddress.Parse(emailMetadata.ToAddress));

            var builder = new BodyBuilder
            {
                HtmlBody = emailMetadata.Body
            };
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(mailSettings.Value.DisplayName, mailSettings.Value.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Value.Host,
            mailSettings.Value.Port,
                SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Value.Email, mailSettings.Value.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

            return "The message was sent successfully...!";
        }
        catch
        {
            throw;
        }
    }
}
