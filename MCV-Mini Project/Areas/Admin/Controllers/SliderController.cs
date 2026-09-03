using MCV_Mini_Project.Areas.Admin.ViewModels.Slider;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        // GET: Admin/Slider
        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            return View(sliders);
        }

        // GET: Admin/Slider/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        // GET: Admin/Slider/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Slider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _sliderService.CreateAsync(vm);
            TempData["Success"] = "Slider uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Slider/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        // POST: Admin/Slider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _sliderService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Slider uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Slider/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sliderService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Slider uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
