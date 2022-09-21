using System.Threading.Tasks;
using Demo.IdentityService.Services.AuthService.Models;

namespace Demo.IdentityService.Services.AuthService;

public interface IAuthService
{
    Task<PasswordLoginResponse> PasswordLoginAsync(string email, string password);
    Task<RequestPasswordResetResponse> RequestPasswordResetAsync(string email);
    Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCodeAsync(string code);
    Task<VerifyPasswordResetCodeResponse> ConfirmPasswordResetAsync(string code, string password);
    Task<VerifyEmailResponse> VerifyEmailAsync(string code);
    Task<ChangePasswordResponse> ChangePasswordAsync(string password, string idToken);
}