using MCV_Mini_Project.Services.Interface;
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

        public IActionResult Index(string? name)
        {
            ViewBag.SearchName = name;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course is null)
                return NotFound();

            ViewBag.Related = (await _courseService.GetAllAsync())
                .Where(c => c.Id != id)
                .Take(3)
                .ToList();

            return View(course);
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            return RedirectToAction(nameof(Index), new { name });
        }
    }
}
