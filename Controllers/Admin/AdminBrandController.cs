using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminBrandController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminBrandController> _logger;

        public AdminBrandController(TelefonOzellikleriDbContext context, ILogger<AdminBrandController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("derin/brands")]
        public async Task<IActionResult> Index(string? search)
        {
            ViewData["Title"] = "Brands";

            var query = _context.Brands.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(b => b.Name.Contains(search) || b.Slug.Contains(search));

            var brands = await query.OrderBy(b => b.Name).ToListAsync();

            var phoneCounts = await _context.Smartphones
                .GroupBy(s => s.BrandId)
                .Select(g => new { BrandId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.BrandId, x => x.Count);

            ViewData["Brands"] = brands;
            ViewData["PhoneCounts"] = phoneCounts;
            ViewData["Search"] = search;

            return View();
        }

        [Route("derin/brands/create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "New Brand";
            ViewData["IsNew"] = true;
            return View("Edit", new Brand());
        }

        [Route("derin/brands/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = "New Brand";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "Name and Slug are required.";
                return View("Edit", model);
            }

            var slugExists = await _context.Brands.AnyAsync(b => b.Slug == model.Slug);
            if (slugExists)
            {
                ViewData["Title"] = "New Brand";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "A brand with this slug already exists.";
                return View("Edit", model);
            }

            _context.Brands.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Brand created: {Id} - {Name}", model.Id, model.Name);
            TempData["Success"] = $"{model.Name} created successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/brands/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            ViewData["Title"] = $"Edit: {brand.Name}";
            return View(brand);
        }

        [Route("derin/brands/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand model)
        {
            if (id != model.Id)
                return BadRequest();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = $"Edit: {brand.Name}";
                ViewData["Error"] = "Name and Slug are required.";
                return View(brand);
            }

            var slugExists = await _context.Brands.AnyAsync(b => b.Slug == model.Slug && b.Id != id);
            if (slugExists)
            {
                ViewData["Title"] = $"Edit: {brand.Name}";
                ViewData["Error"] = "A brand with this slug already exists.";
                return View(brand);
            }

            brand.Name = model.Name;
            brand.Slug = model.Slug;
            brand.LogoUrl = model.LogoUrl;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Brand updated: {Id} - {Name}", brand.Id, brand.Name);

            TempData["Success"] = $"{brand.Name} updated successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/brands/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            var phoneCount = await _context.Smartphones.CountAsync(s => s.BrandId == id);
            if (phoneCount > 0)
            {
                TempData["Error"] = $"{brand.Name} cannot be deleted because it has {phoneCount} phone(s) linked to it.";
                return RedirectToAction("Index");
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Brand deleted: {Id} - {Name}", id, brand.Name);

            TempData["Success"] = $"{brand.Name} deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
