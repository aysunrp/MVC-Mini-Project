using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class CoursePageHeroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title = "Courses", string? current = null)
        {
            ViewBag.HeroTitle = title;
            ViewBag.HeroCurrent = current;
            return View();
        }
    }
}
