using Demo.IdentityService.Models;
using Demo.IdentityService.Services.AuthService;
using Demo.IdentityService.Services.EmailService;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Demo.IdentityService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            var result = await _authService.PasswordLoginAsync(model.Email, model.Password);
            return Ok(new
            {
                result.IdToken, result.RefreshToken,
            });
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            var auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
            var result = await auth.CreateUserAsync(new UserRecordArgs
            {
                Email = model.Email,
                Password = model.Password,
            });
            
            // add additional claims here
            var claims = new Dictionary<string, object>();
            claims.Add("name", model.Name);
            await auth.SetCustomUserClaimsAsync(result.Uid, claims);
            
            var link= await auth.GenerateEmailVerificationLinkAsync(model.Email);
            
            // send email here
            await _emailService.SendAsync(
                email: model.Email, 
                subject: "Verify your account", 
                message: $"Please verify your account by clicking this link: {link}");
            
            var loginResponse = await _authService.PasswordLoginAsync(model.Email, model.Password);
            return Ok(new
            {
                loginResponse.IdToken, loginResponse.RefreshToken,
            });
        }
        
        [HttpPost]
        [Route("change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel model)
        {
            var result = await  _authService.ChangePasswordAsync(model.Password, model.IdToken);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("verify_email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailInputModel model)
        {
            await _authService.VerifyEmailAsync(model.Code);
            return Ok();
        }
        
        [HttpPost]
        [Route("reset_password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel model)
        {
            var auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
            
            var link = await auth.GeneratePasswordResetLinkAsync(model.Email);
            
            // send email here
            await _emailService.SendAsync(
                email: model.Email, 
                subject: "Reset your password", 
                message: $"Please reset your password by clicking this link: {link}");
            
            return Ok();
        }
        
        [HttpPost]
        [Route("reset_password_verify")]
        public async Task<IActionResult> VerifyResetPasswordCode([FromBody] VerifyResetPasswordCodeInputModel model)
        {
            var result = await _authService.VerifyPasswordResetCodeAsync(model.Code);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("reset_password_confirm")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordInputModel model)
        {
            var result = await _authService.ConfirmPasswordResetAsync(model.Code, model.Password);
            return Ok(result);
        }
    }
}