namespace Demo.Api.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public string GetUserIdentity()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value ?? string.Empty;
    }
    
    public IdentityUser GetApplicationUser()
    {
        var claims = _httpContextAccessor.HttpContext.User;

        return new IdentityUser
        {
            Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty,
            Id = claims.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value ?? string.Empty,
            Firstname = claims.Claims.FirstOrDefault(x => x.Type == "first_name")?.Value ?? string.Empty,
            Lastname = claims.Claims.FirstOrDefault(x => x.Type == "last_name")?.Value ?? string.Empty,
            EmailVerified = claims.Claims.FirstOrDefault(x => x.Type == "email_verified")?.Value == "true"
        };
    }

}