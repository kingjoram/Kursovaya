using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ILogger<HomeController> logger, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // var appRole = new AppRole("Admin");
        // var admin = await _roleManager.CreateAsync(new AppRole("Admin"));
        // var zuser = await _roleManager.CreateAsync(new AppRole("User"));
        // var firstOrDefault = _userManager.Users.FirstOrDefault(x => x.Id == "83d2e96b-050a-4007-9c7a-0aac776339d7");

        // var addToRoleAsync = await _userManager.AddToRoleAsync(firstOrDefault, "Admin");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}