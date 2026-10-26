using System.ComponentModel.DataAnnotations;

namespace DeviceManagementMVC.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(25)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;
}
