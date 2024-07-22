
using Application.Email;

namespace Application.Contracts;

public interface IEmailSenderServiceAsync
{
    Task<string> SendAsync(EmailMetadata emailMetadata);
}
