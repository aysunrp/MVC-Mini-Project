using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
