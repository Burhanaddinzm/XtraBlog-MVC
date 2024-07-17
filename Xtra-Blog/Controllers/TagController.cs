using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class TagController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
