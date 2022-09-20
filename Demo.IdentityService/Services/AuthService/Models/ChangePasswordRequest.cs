using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class ChangePasswordRequest
{
    [JsonProperty("password")]
    public string Password { get; set; }
    [JsonProperty("idToken")]
    public string IdToken { get; set; }
    [JsonProperty("returnSecureToken")]
    public bool ReturnSecureToken { get; init; }
}