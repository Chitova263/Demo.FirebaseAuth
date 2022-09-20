using System.ComponentModel.DataAnnotations;

namespace Demo.IdentityService.Models;

public class RegisterInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(100)]
    public string Password { get; set; }
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
}