using MCV_Mini_Project.Areas.Admin.ViewModels.Event;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: Admin/Event
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }

        // GET: Admin/Event/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev is null) return NotFound();
            return View(ev);
        }

        // GET: Admin/Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _eventService.CreateAsync(vm);
            TempData["Success"] = "Tədbir uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Event/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev is null) return NotFound();
            return View(ev);
        }

        // POST: Admin/Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _eventService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Tədbir uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _eventService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Tədbir uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
