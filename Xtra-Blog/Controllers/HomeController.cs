using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
