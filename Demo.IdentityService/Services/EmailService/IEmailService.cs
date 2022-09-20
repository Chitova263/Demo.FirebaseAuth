namespace Demo.IdentityService.Services.EmailService;

public interface IEmailService
{
    Task SendAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
}