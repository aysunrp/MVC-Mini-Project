using MCV_Mini_Project.Areas.Admin.ViewModels.AboutPlatform;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutPlatformController : Controller
    {
        private readonly IPlatformService _platformService;

        public AboutPlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        // GET: Admin/AboutPlatform
        public async Task<IActionResult> Index()
        {
            var platforms = await _platformService.GetAllAdminAsync();
            return View(platforms);
        }

        // GET: Admin/AboutPlatform/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var platform = await _platformService.GetByIdAsync(id);
            if (platform is null) return NotFound();
            return View(platform);
        }

        // GET: Admin/AboutPlatform/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutPlatform/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutPlatformCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _platformService.CreateAsync(vm);
            TempData["Success"] = "Platform haqqında məlumat uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AboutPlatform/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var platform = await _platformService.GetByIdAsync(id);
            if (platform is null) return NotFound();
            return View(platform);
        }

        // POST: Admin/AboutPlatform/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AboutPlatformEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _platformService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Platform haqqında məlumat uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AboutPlatform/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _platformService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Platform haqqında məlumat uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
