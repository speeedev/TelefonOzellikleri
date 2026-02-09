using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminSeriesController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminSeriesController> _logger;

        public AdminSeriesController(TelefonOzellikleriDbContext context, ILogger<AdminSeriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("derin/series")]
        public async Task<IActionResult> Index(string? search, int? brandId)
        {
            ViewData["Title"] = "Series";

            var query = _context.Series.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(s => s.SeriesName.Contains(search) || s.Slug.Contains(search));

            if (brandId.HasValue)
                query = query.Where(s => s.BrandId == brandId.Value);

            var series = await query
                .Include(s => s.Brand)
                .OrderBy(s => s.SeriesName)
                .ToListAsync();

            var brands = await _context.Brands.OrderBy(b => b.Name).ToListAsync();

            ViewData["Series"] = series;
            ViewData["Brands"] = brands;
            ViewData["Search"] = search;
            ViewData["BrandId"] = brandId;

            return View();
        }

        [Route("derin/series/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "New Series";
            ViewData["IsNew"] = true;
            await PopulateDropdowns();
            return View("Edit", new Series());
        }

        [Route("derin/series/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Series model)
        {
            if (string.IsNullOrWhiteSpace(model.SeriesName) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = "New Series";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "Series Name and Slug are required.";
                await PopulateDropdowns(model.BrandId);
                return View("Edit", model);
            }

            var slugExists = await _context.Series.AnyAsync(s => s.Slug == model.Slug);
            if (slugExists)
            {
                ViewData["Title"] = "New Series";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "A series with this slug already exists.";
                await PopulateDropdowns(model.BrandId);
                return View("Edit", model);
            }

            _context.Series.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Series created: {Id} - {SeriesName}", model.Id, model.SeriesName);
            TempData["Success"] = $"{model.SeriesName} created successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/series/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series == null)
                return NotFound();

            ViewData["Title"] = $"Edit: {series.SeriesName}";
            await PopulateDropdowns(series.BrandId);

            return View(series);
        }

        [Route("derin/series/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Series model)
        {
            if (id != model.Id)
                return BadRequest();

            var series = await _context.Series.FindAsync(id);
            if (series == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(model.SeriesName) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = $"Edit: {series.SeriesName}";
                ViewData["Error"] = "Series Name and Slug are required.";
                await PopulateDropdowns(model.BrandId);
                return View(model);
            }

            var slugExists = await _context.Series.AnyAsync(s => s.Slug == model.Slug && s.Id != id);
            if (slugExists)
            {
                ViewData["Title"] = $"Edit: {series.SeriesName}";
                ViewData["Error"] = "A series with this slug already exists.";
                await PopulateDropdowns(model.BrandId);
                return View(model);
            }

            series.BrandId = model.BrandId;
            series.SeriesName = model.SeriesName;
            series.Slug = model.Slug;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Series updated: {Id} - {SeriesName}", series.Id, series.SeriesName);

            TempData["Success"] = $"{series.SeriesName} updated successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/series/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series == null)
                return NotFound();

            var hasPhones = await _context.Smartphones.AnyAsync(s => s.SeriesId == id);
            if (hasPhones)
            {
                TempData["Error"] = "Cannot delete series that has associated phones.";
                return RedirectToAction("Index");
            }

            _context.Series.Remove(series);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Series deleted: {Id} - {SeriesName}", series.Id, series.SeriesName);
            TempData["Success"] = $"{series.SeriesName} deleted successfully.";
            return RedirectToAction("Index");
        }

        private async Task PopulateDropdowns(int? selectedBrandId = null)
        {
            var brands = await _context.Brands.OrderBy(b => b.Name).ToListAsync();
            ViewData["BrandList"] = new SelectList(brands, "Id", "Name", selectedBrandId);
        }
    }
}
