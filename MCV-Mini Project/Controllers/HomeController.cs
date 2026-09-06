using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

