using MCV_Mini_Project.Areas.Admin.ViewModels.Setting;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // GET: Admin/Setting
        public async Task<IActionResult> Index()
        {
            var settings = await _settingService.GetAllAsync();
            return View(settings);
        }

        // GET: Admin/Setting/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var setting = await _settingService.GetByIdAsync(id);
            if (setting is null) return NotFound();
            return View(setting);
        }

        // GET: Admin/Setting/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Setting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _settingService.CreateAsync(vm);
            TempData["Success"] = "Parametr uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Setting/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var setting = await _settingService.GetByIdAsync(id);
            if (setting is null) return NotFound();
            return View(setting);
        }

        // POST: Admin/Setting/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _settingService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Parametr uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Setting/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _settingService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Parametr uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
