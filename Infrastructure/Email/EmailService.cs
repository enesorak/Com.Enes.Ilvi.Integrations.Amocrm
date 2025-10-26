using Application.Abstractions.Email;

namespace Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(string recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}