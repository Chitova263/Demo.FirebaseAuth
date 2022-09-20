using Newtonsoft.Json;

namespace Demo.IdentityService.Services.AuthService.Models;

public class ConfirmPasswordResetRequest
{
    [JsonProperty("oobCode")]
    public string OobCode { get; set; }
    [JsonProperty("newPassword")]
    public string NewPassword { get; set; }

}