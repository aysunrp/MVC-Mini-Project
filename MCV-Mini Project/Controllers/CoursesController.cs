using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();

            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            var courses = await _courseService.SearchAsync(name);

            return View("Index", courses);
        }
    }
}