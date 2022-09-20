namespace Demo.IdentityService.Models;

public class ChangePasswordInputModel
{
    public string IdToken { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}