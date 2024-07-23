using Application.Contracts;
using Application.Email;
using Application.Services;

namespace Infrastructure.BackgroundServices;

public class RemainderEmailService(IUserRepositoryAsync userRepositoryAsync, IEmailSenderServiceAsync emailSenderServiceAsync)
{
    public async Task SendForHasNotLoginUsersBeforeAsync()
    {
        var hasNotFirstLoginEmails = await userRepositoryAsync.GetHasNotFirstLoginUsersEmailsAsync();

        var sendingEmailsTasks = hasNotFirstLoginEmails
            .Select(receiverEmail => new EmailMetadata(
                receiverEmail,
                Subject: "Login Remaindering",
                Body: "We note that you did not login until now, so we remaindering and proms you first try will be perfect."))
            .Select(async emailMetadata => await emailSenderServiceAsync.SendAsync(emailMetadata))
            .ToArray();

        Task.WaitAll(sendingEmailsTasks);
    } 
}
