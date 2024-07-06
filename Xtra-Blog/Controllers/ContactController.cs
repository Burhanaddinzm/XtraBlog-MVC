using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
