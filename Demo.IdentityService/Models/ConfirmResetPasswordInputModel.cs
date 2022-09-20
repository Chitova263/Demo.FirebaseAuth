namespace Demo.IdentityService.Models;

public class ConfirmResetPasswordInputModel
{
    public string Code { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}