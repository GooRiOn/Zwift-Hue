using System.ComponentModel.DataAnnotations;

namespace ZwiftHue.UI.Models.Write;

public class RiderLoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}