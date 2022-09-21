using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Demo.IdentityService.Config;
using Demo.IdentityService.Services.AuthService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly FirebaseConfiguration _firebaseConfiguration;

    public AuthService(HttpClient httpClient, IOptions<FirebaseConfiguration> options, ILogger<AuthService> logger)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _firebaseConfiguration = options.Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
    }

    public async Task<PasswordLoginResponse> PasswordLoginAsync(string email, string password)
    {
        var uri = $"/v1/accounts:signInWithPassword?key={_firebaseConfiguration.WebApiKey}";
        var json = JsonConvert.SerializeObject(new LoginRequest{Email = email, Password = password, ReturnSecureToken = true});
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, request);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PasswordLoginResponse>(content);
    }

    public async Task<RequestPasswordResetResponse> RequestPasswordResetAsync(string email)
    {
        var uri = $"/v1/accounts:sendOobCode?key=${_firebaseConfiguration.WebApiKey}";
        var json = JsonConvert.SerializeObject(new PasswordResetRequest
        {
            Email = email,
            RequestType = "PASSWORD_RESET"
        });
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(uri, request);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RequestPasswordResetResponse>(content);
    }

    public async Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCodeAsync(string code)
    {
        var uri = $"/v1/accounts:resetPassword?key={_firebaseConfiguration.WebApiKey}";
        var json = JsonConvert.SerializeObject(new VerifyPasswordResetCodeRequest
        {
            OobCode = code
        });
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, request);

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<VerifyPasswordResetCodeResponse>(content);
    }

    public async Task<VerifyPasswordResetCodeResponse> ConfirmPasswordResetAsync(string code, string password)
    {
        var uri = $"/v1/accounts:resetPassword?key={_firebaseConfiguration.WebApiKey}";
        
        var json = JsonConvert.SerializeObject(new ConfirmPasswordResetRequest
        {
            OobCode = code,
            NewPassword = password
        });
        
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, request);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<VerifyPasswordResetCodeResponse>(content);
    }

    public async Task<VerifyEmailResponse> VerifyEmailAsync(string code)
    {
        var uri = $"/v1/accounts:update?key=${_firebaseConfiguration.WebApiKey}";
        var json = JsonConvert.SerializeObject(new VerifyEmailRequest
        {
            OobCode = code
        });
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(uri, request);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<VerifyEmailResponse>(content);
    }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(string password, string idToken)
    {
        var uri = $"/v1/accounts:update?key={_firebaseConfiguration.WebApiKey}";
        
        var json = JsonConvert.SerializeObject(new ChangePasswordRequest
        {
            IdToken = idToken,
            Password = password,
            ReturnSecureToken = true
        });   
        
        var request = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, request);

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangePasswordResponse>(content);
    }
}