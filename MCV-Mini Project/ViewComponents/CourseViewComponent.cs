using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly ICourseService _courseService;

        public CourseViewComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }
    }
}
