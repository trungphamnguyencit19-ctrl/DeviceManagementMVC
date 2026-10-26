using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Devices");
}
