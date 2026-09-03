using MCV_Mini_Project.Areas.Admin.ViewModels.Teacher;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService   _teacherService;
        private readonly IPositionService  _positionService;

        public TeacherController(ITeacherService teacherService, IPositionService positionService)
        {
            _teacherService  = teacherService;
            _positionService = positionService;
        }

        // GET: Admin/Teacher
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllAsync();
            return View(teachers);
        }

        // GET: Admin/Teacher/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher is null) return NotFound();
            return View(teacher);
        }

        // GET: Admin/Teacher/Create
        public async Task<IActionResult> Create()
        {
            await PopulatePositionsDropdownAsync();
            return View();
        }

        // POST: Admin/Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePositionsDropdownAsync();
                return View(vm);
            }

            await _teacherService.CreateAsync(vm);
            TempData["Success"] = "Müəllim uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Teacher/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher is null) return NotFound();

            await PopulatePositionsDropdownAsync(teacher.PositionId);
            return View(teacher);
        }

        // POST: Admin/Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePositionsDropdownAsync(vm.PositionId);
                return View(vm);
            }

            var result = await _teacherService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Müəllim uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Teacher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Müəllim uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulatePositionsDropdownAsync(int? selectedId = null)
        {
            var positions = await _positionService.GetAllAsync();
            ViewBag.Positions = new SelectList(positions, "Id", "Name", selectedId);
        }
    }
}
