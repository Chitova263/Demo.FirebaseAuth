namespace Demo.Api.Services;

public interface IIdentityService
{
    IdentityUser GetApplicationUser();
    string GetUserIdentity();

}