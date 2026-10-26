using DeviceManagementMVC.Data;
using DeviceManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementMVC.Controllers;

public class DevicesController : Controller
{
    private readonly ApplicationDbContext _context;
    public DevicesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search, DeviceStatus? status, int? categoryId)
    {
        var query = _context.Devices.Include(d => d.Category).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(d => d.Name.Contains(search) || d.Code.Contains(search));
        if (status.HasValue) query = query.Where(d => d.Status == status.Value);
        if (categoryId.HasValue) query = query.Where(d => d.CategoryId == categoryId.Value);
        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.CategoryId = categoryId;
        ViewBag.Categories = new SelectList(await _context.DeviceCategories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", categoryId);
        return View(await query.OrderBy(d => d.Name).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View(new Device { DateOfEntry = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Device device)
    {
        if (await _context.Devices.AnyAsync(d => d.Code == device.Code))
            ModelState.AddModelError(nameof(device.Code), "This device code already exists.");
        if (!await _context.DeviceCategories.AnyAsync(c => c.Id == device.CategoryId))
            ModelState.AddModelError(nameof(device.CategoryId), "Choose a valid category.");
        if (!ModelState.IsValid) { await LoadCategories(device.CategoryId); return View(device); }
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var device = await _context.Devices.FindAsync(id);
        if (device is null) return NotFound();
        await LoadCategories(device.CategoryId);
        return View(device);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Device device)
    {
        if (id != device.Id) return NotFound();
        if (await _context.Devices.AnyAsync(d => d.Code == device.Code && d.Id != id))
            ModelState.AddModelError(nameof(device.Code), "This device code already exists.");
        if (!ModelState.IsValid) { await LoadCategories(device.CategoryId); return View(device); }
        _context.Update(device);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var device = await _context.Devices.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
        return device is null ? NotFound() : View(device);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device is null) return NotFound();
        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategories(int? selectedId = null)
    {
        ViewBag.CategoryId = new SelectList(
            await _context.DeviceCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(),
            "Id", "Name", selectedId);
    }
}
