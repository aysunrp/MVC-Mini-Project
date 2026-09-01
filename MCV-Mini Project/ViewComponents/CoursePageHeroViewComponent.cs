using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class CoursePageHeroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
