using DeviceManagementMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementMVC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<DeviceCategory> DeviceCategories => Set<DeviceCategory>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DeviceCategory>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Device>().HasIndex(d => d.Code).IsUnique();
        modelBuilder.Entity<Device>()
            .HasOne(d => d.Category)
            .WithMany(c => c.Devices)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
