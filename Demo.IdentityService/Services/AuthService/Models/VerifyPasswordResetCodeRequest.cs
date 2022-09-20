using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class VerifyPasswordResetCodeRequest
{
    [JsonProperty("oobCode")]
    public string OobCode { get; set; }
}