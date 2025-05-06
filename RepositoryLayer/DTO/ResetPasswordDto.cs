using System.ComponentModel.DataAnnotations;

public class ResetPasswordDto
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }

    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
