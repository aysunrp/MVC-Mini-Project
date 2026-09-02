using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class TeacherViewComponent :ViewComponent
    {
        private readonly ITeacherService _teacherService;
        public TeacherViewComponent(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teachers = await _teacherService.GetAllUIAsync();
            return View(teachers);
        }
    }
}
