using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class VerifyEmailRequest
{
    [JsonProperty("oobCode")]
    public string OobCode { get; set; }
}