using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
