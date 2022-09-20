using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class LoginRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
    [JsonProperty("returnSecureToken")]
    public bool ReturnSecureToken { get; set; }
}