using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class FeaturedViewComponent : ViewComponent
    {
        private readonly ICourseService _courseService;

        public FeaturedViewComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = (await _courseService.GetAllAsync()).ToList();
            var featured = courses.FirstOrDefault(x => x.IsFeatured) ?? courses.FirstOrDefault();
            return View(featured);
        }
    }
}
