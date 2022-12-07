using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class IncomingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}