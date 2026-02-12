using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminPageController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminPageController> _logger;

        public AdminPageController(TelefonOzellikleriDbContext context, ILogger<AdminPageController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("derin/pages")]
        public async Task<IActionResult> Index(string? search)
        {
            ViewData["Title"] = "Pages";

            var query = _context.Pages.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.PageTitle.Contains(search) || p.Slug.Contains(search));

            var pages = await query.OrderByDescending(p => p.UpdatedAt).ToListAsync();

            ViewData["Pages"] = pages;
            ViewData["Search"] = search;

            return View();
        }

        [Route("derin/pages/create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "New Page";
            return View("Edit", new Page());
        }

        [Route("derin/pages/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page model)
        {
            if (string.IsNullOrWhiteSpace(model.PageTitle) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = "New Page";
                ViewData["Error"] = "Title and Slug are required.";
                return View("Edit", model);
            }

            var slugAvailable = await _context.IsSlugAvailableAsync(model.Slug);
            if (!slugAvailable)
            {
                ViewData["Title"] = "New Page";
                ViewData["Error"] = "This slug is already used by a page, phone, or brand. Slugs must be unique across the site.";
                return View("Edit", model);
            }

            model.UpdatedAt = DateTime.Now;
            _context.Pages.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Page created: {Id} - {Title}", model.Id, model.PageTitle);
            TempData["Success"] = $"{model.PageTitle} created successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/pages/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
                return NotFound();

            ViewData["Title"] = $"Edit: {page.PageTitle}";
            return View(page);
        }

        [Route("derin/pages/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Page model)
        {
            if (id != model.Id)
                return BadRequest();

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(model.PageTitle) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = $"Edit: {page.PageTitle}";
                ViewData["Error"] = "Title and Slug are required.";
                return View(page);
            }

            var slugAvailable = await _context.IsSlugAvailableAsync(model.Slug, excludePageId: id);
            if (!slugAvailable)
            {
                ViewData["Title"] = $"Edit: {page.PageTitle}";
                ViewData["Error"] = "This slug is already used by a page, phone, or brand. Slugs must be unique across the site.";
                return View(page);
            }

            page.PageTitle = model.PageTitle;
            page.Slug = model.Slug;
            page.PageDescription = model.PageDescription;
            page.Content = model.Content;
            page.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Page updated: {Id} - {Title}", page.Id, page.PageTitle);

            TempData["Success"] = $"{page.PageTitle} updated successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/pages/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
                return NotFound();

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Page deleted: {Id} - {Title}", id, page.PageTitle);

            TempData["Success"] = $"{page.PageTitle} deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
