using MCV_Mini_Project.Areas.Admin.ViewModels.Author;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Policies.AdminPanel)]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: Admin/Author
        public async Task<IActionResult> Index()
        {
            var authors = await _authorService.GetAllAsync();
            return View(authors);
        }

        // GET: Admin/Author/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author is null) return NotFound();
            return View(author);
        }

        // GET: Admin/Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _authorService.CreateAsync(vm);
            TempData["Success"] = "Müəllif uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Author/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author is null) return NotFound();
            return View(author);
        }

        // POST: Admin/Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _authorService.UpdateAsync(vm);
            if (!result) return NotFound();

            TempData["Success"] = "Müəllif uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["Success"] = "Müəllif uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
