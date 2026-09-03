using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
