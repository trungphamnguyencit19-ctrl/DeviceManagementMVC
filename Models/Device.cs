using System.ComponentModel.DataAnnotations;

namespace DeviceManagementMVC.Models;

public class Device
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Device name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(40)]
    [Display(Name = "Device code")]
    public string Code { get; set; } = string.Empty;

    [Required, Display(Name = "Device category")]
    public int CategoryId { get; set; }

    public DeviceCategory? Category { get; set; }

    [Required]
    public DeviceStatus Status { get; set; } = DeviceStatus.InUse;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Date of entry")]
    public DateTime DateOfEntry { get; set; } = DateTime.Today;
}

public enum DeviceStatus
{
    [Display(Name = "In use")] InUse,
    [Display(Name = "Broken")] Broken,
    [Display(Name = "Under maintenance")] UnderMaintenance
}
