using Microsoft.AspNetCore.Mvc;
using MCV_Mini_Project.Services.Interface;

namespace MCV_Mini_Project.ViewComponents
{
    public class CoursePageViewComponent : ViewComponent
    {
        private readonly ICourseService _courseService;

        public CoursePageViewComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? name = null)
        {
            var courses = string.IsNullOrWhiteSpace(name)
                ? await _courseService.GetAllAsync()
                : await _courseService.SearchAsync(name);

            ViewBag.SearchTerm = name?.Trim();
            return View(courses);
        }
    }
}