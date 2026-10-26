using System.ComponentModel.DataAnnotations;

namespace DeviceManagementMVC.Models;

public class DeviceCategory
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Category name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
