using System.ComponentModel.DataAnnotations;

namespace Demo.IdentityService.Models;

public class LoginInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(100)]
    public string Password { get; set; }
}