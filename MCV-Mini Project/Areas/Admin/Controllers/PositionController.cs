using MCV_Mini_Project.Areas.Admin.ViewModels.Position;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.AdminPanel)]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        // GET: Admin/Position
        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllAsync();
            return View(positions);
        }

        // GET: Admin/Position/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var position = await _positionService.GetByIdAsync(id);
            if (position is null) return NotFound();
            return View(position);
        }

        // GET: Admin/Position/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Position/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _positionService.CreateAsync(vm);
            TempData["Success"] = "Vəzifə uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Position/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var position = await _positionService.GetByIdAsync(id);
            if (position is null) return NotFound();
            return View(position);
        }

        // POST: Admin/Position/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PositionEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _positionService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Vəzifə uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Position/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _positionService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Vəzifə uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
