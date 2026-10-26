using DeviceManagementMVC.Data;
using DeviceManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementMVC.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    public UsersController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Users.AsNoTracking().OrderBy(u => u.FullName).ToListAsync());

    public IActionResult Create() => View(new AppUser());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppUser user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            ModelState.AddModelError(nameof(user.Email), "This email is already registered.");
        if (!ModelState.IsValid) return View(user);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppUser user)
    {
        if (id != user.Id) return NotFound();
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != id))
            ModelState.AddModelError(nameof(user.Email), "This email is already registered.");
        if (!ModelState.IsValid) return View(user);
        _context.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
