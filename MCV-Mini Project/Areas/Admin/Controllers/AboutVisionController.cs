using MCV_Mini_Project.Areas.Admin.ViewModels.AboutVision;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutVisionController : Controller
    {
        private readonly IVisionService _visionService;

        public AboutVisionController(IVisionService visionService)
        {
            _visionService = visionService;
        }

        // GET: Admin/AboutVision
        public async Task<IActionResult> Index()
        {
            var visions = await _visionService.GetAllAdminAsync();
            return View(visions);
        }

        // GET: Admin/AboutVision/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vision = await _visionService.GetByIdAsync(id);
            if (vision is null) return NotFound();
            return View(vision);
        }

        // GET: Admin/AboutVision/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutVision/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutVisionCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _visionService.CreateAsync(vm);
            TempData["Success"] = "Vizyon haqqında məlumat uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AboutVision/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vision = await _visionService.GetByIdAsync(id);
            if (vision is null) return NotFound();
            return View(vision);
        }

        // POST: Admin/AboutVision/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AboutVisionEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _visionService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Vizyon haqqında məlumat uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AboutVision/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _visionService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Vizyon haqqında məlumat uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
