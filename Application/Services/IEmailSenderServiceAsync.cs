using Application.Email;

namespace Application.Services;

public interface IEmailSenderServiceAsync
{
    Task<string> SendAsync(EmailMetadata emailMetadata);
}
