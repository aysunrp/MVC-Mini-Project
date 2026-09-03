using MCV_Mini_Project.Areas.Admin.ViewModels.News;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IAuthorService _authorService;

        public NewsController(INewsService newsService, IAuthorService authorService)
        {
            _newsService   = newsService;
            _authorService = authorService;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetAllAsync();
            return View(news);
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news is null) return NotFound();
            return View(news);
        }

        // GET: Admin/News/Create
        public async Task<IActionResult> Create()
        {
            await PopulateAuthorsDropdownAsync();
            return View();
        }

        // POST: Admin/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAuthorsDropdownAsync();
                return View(vm);
            }

            await _newsService.CreateAsync(vm);
            TempData["Success"] = "Xəbər uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news is null) return NotFound();

            await PopulateAuthorsDropdownAsync(news.AuthorId);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAuthorsDropdownAsync(vm.AuthorId);
                return View(vm);
            }

            var result = await _newsService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Xəbər uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Xəbər uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAuthorsDropdownAsync(int? selectedId = null)
        {
            var authors = await _authorService.GetAllAsync();
            ViewBag.Authors = new SelectList(authors, "Id", "FullName", selectedId);
        }
    }
}
