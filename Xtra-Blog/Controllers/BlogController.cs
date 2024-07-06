using Microsoft.AspNetCore.Mvc;

namespace XtraBlog.Controllers;

public class BlogController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int? id)
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    public async Task<IActionResult> Update(int? id)
    {
        return View();
    }
}
