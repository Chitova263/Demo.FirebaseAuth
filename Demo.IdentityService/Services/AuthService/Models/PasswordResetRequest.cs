using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class PasswordResetRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("requestType")]
    public string RequestType { get; set; }
}