using MCV_Mini_Project.Areas.Admin.ViewModels.CourseInfo;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseInfoController : Controller
    {
        private readonly ICourseInfoService _courseInfoService;
        private readonly ITeacherService    _teacherService;

        public CourseInfoController(ICourseInfoService courseInfoService, ITeacherService teacherService)
        {
            _courseInfoService = courseInfoService;
            _teacherService    = teacherService;
        }

        // GET: Admin/CourseInfo
        public async Task<IActionResult> Index()
        {
            var courses = await _courseInfoService.GetAllAsync();
            return View(courses);
        }

        // GET: Admin/CourseInfo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseInfoService.GetByIdAsync(id);
            if (course is null) return NotFound();
            return View(course);
        }

        // GET: Admin/CourseInfo/Create
        public async Task<IActionResult> Create()
        {
            await PopulateTeachersDropdownAsync();
            return View();
        }

        // POST: Admin/CourseInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseInfoCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTeachersDropdownAsync();
                return View(vm);
            }

            await _courseInfoService.CreateAsync(vm);
            TempData["Success"] = "Kurs uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/CourseInfo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseInfoService.GetByIdAsync(id);
            if (course is null) return NotFound();

            await PopulateTeachersDropdownAsync(course.TeacherId);
            return View(course);
        }

        // POST: Admin/CourseInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseInfoEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTeachersDropdownAsync(vm.TeacherId);
                return View(vm);
            }

            var result = await _courseInfoService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Kurs uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/CourseInfo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseInfoService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Kurs uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateTeachersDropdownAsync(int? selectedId = null)
        {
            var teachers = await _teacherService.GetAllAsync();
            ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", selectedId);
        }
    }
}
