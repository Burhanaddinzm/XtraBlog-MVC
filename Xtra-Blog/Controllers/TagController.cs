using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class TagController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(int test)
    {
        return View();
    }
}
