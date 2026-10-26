using DeviceManagementMVC.Data;
using DeviceManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;
    public CategoriesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.DeviceCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync());

    public IActionResult Create() => View(new DeviceCategory());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DeviceCategory category)
    {
        if (await _context.DeviceCategories.AnyAsync(c => c.Name == category.Name))
            ModelState.AddModelError(nameof(category.Name), "This category already exists.");
        if (!ModelState.IsValid) return View(category);
        _context.DeviceCategories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var category = await _context.DeviceCategories.FindAsync(id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DeviceCategory category)
    {
        if (id != category.Id) return NotFound();
        if (await _context.DeviceCategories.AnyAsync(c => c.Name == category.Name && c.Id != id))
            ModelState.AddModelError(nameof(category.Name), "This category already exists.");
        if (!ModelState.IsValid) return View(category);
        _context.Update(category);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Cannot save this category while it is in use."); return View(category); }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var category = await _context.DeviceCategories.FirstOrDefaultAsync(c => c.Id == id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.DeviceCategories.Include(c => c.Devices).FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) return NotFound();
        if (category.Devices.Any())
        {
            TempData["Error"] = "This category has devices. Move or delete those devices first.";
            return RedirectToAction(nameof(Index));
        }
        _context.DeviceCategories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
