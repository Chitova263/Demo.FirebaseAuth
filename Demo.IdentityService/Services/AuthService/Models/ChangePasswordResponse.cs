using System.Collections.Generic;

namespace Demo.IdentityService.Services.AuthService.Models;

public class ChangePasswordResponse
{
    public string LocalId { get; set; }
    public string Email { get; set; }
    public List<string> ProviderUserInfo { get; set; }
    public string IdToken { get; set; }
    public string RefreshToken { get; set; }
    public string ExpiresIn { get; set; }
}