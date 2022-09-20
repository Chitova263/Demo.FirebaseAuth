namespace Demo.IdentityService.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task SendAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Sending email to {email} with subject {subject} and message {message}");
        
        return Task.CompletedTask;
    }
}