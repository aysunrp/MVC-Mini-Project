using MCV_Mini_Project.Areas.Admin.ViewModels.Icon;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.AdminPanel)]
    public class IconController : Controller
    {
        private readonly IIconService _iconService;

        public IconController(IIconService iconService)
        {
            _iconService = iconService;
        }

        // GET: Admin/Icon
        public async Task<IActionResult> Index()
        {
            var icons = await _iconService.GetAllAsync();
            return View(icons);
        }

        // GET: Admin/Icon/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var icon = await _iconService.GetByIdAsync(id);
            if (icon is null) return NotFound();
            return View(icon);
        }

        // GET: Admin/Icon/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Icon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IconCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _iconService.CreateAsync(vm);
            TempData["Success"] = "Icon uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Icon/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var icon = await _iconService.GetByIdAsync(id);
            if (icon is null) return NotFound();
            return View(icon);
        }

        // POST: Admin/Icon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IconEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _iconService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Icon uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Icon/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _iconService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Icon uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
