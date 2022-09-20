namespace Demo.Api.Services;

public class IdentityUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public bool EmailVerified { get; set; }
}