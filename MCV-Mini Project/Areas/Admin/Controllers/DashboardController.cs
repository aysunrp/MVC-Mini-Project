using MCV_Mini_Project.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.AdminPanel)]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
