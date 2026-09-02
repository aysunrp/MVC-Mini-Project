using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            return RedirectToAction(nameof(Index), new { name });
        }
    }
}