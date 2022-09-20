namespace Demo.IdentityService.Services.AuthService.Models;

public class VerifyEmailResponse
{
    public string Email { get; set; }
    public string Displayname { get; set; }
    public string PhotoUrl { get; set; }
    public string PasswordHash { get; set; }
    public string EmailVerified { get; set; }
    public List<string> ProviderUserInfo { get; set; }
}
