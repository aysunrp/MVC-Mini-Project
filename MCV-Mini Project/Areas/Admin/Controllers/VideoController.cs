using MCV_Mini_Project.Areas.Admin.ViewModels.Video;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        // GET: Admin/Video
        public async Task<IActionResult> Index()
        {
            var videos = await _videoService.GetAllAsync();
            return View(videos);
        }

        // GET: Admin/Video/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var video = await _videoService.GetByIdAsync(id);
            if (video is null) return NotFound();
            return View(video);
        }

        // GET: Admin/Video/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Video/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _videoService.CreateAsync(vm);
            TempData["Success"] = "Video uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Video/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var video = await _videoService.GetByIdAsync(id);
            if (video is null) return NotFound();
            return View(video);
        }

        // POST: Admin/Video/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VideoEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _videoService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Video uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Video/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _videoService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Video uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
