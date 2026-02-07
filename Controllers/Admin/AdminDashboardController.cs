using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;

        public AdminDashboardController(TelefonOzellikleriDbContext context)
        {
            _context = context;
        }

        [Route("derin")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";

            ViewData["PhoneCount"] = await _context.Smartphones.CountAsync();
            ViewData["BrandCount"] = await _context.Brands.CountAsync();
            ViewData["PageCount"] = await _context.Pages.CountAsync();

            return View();
        }
    }
}
