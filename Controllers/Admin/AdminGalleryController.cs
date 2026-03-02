using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TelefonOzellikleri.Controllers.Admin;

[Authorize]
public class AdminGalleryController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<AdminGalleryController> _logger;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };
    private static readonly string[] GalleryFolders = { "phones", "brands", "pages", "misc" };

    public AdminGalleryController(IWebHostEnvironment env, ILogger<AdminGalleryController> logger)
    {
        _env = env;
        _logger = logger;
    }

    [Route("derin/gallery")]
    [HttpGet]
    [ResponseCache(NoStore = true, Duration = 0)] 
    public IActionResult Index([FromQuery(Name = "folder")] string? folder = "phones")
    {
        ViewData["Title"] = "Gallery";

        var folderLower = (folder ?? "").ToLowerInvariant();
        var selectedFolder = GalleryFolders.Contains(folderLower) ? folderLower : "phones";
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", selectedFolder);

        Directory.CreateDirectory(uploadsPath);

        var files = new List<GalleryFileInfo>();
        if (Directory.Exists(uploadsPath))
        {
            foreach (var filePath in Directory.GetFiles(uploadsPath))
            {
                var ext = Path.GetExtension(filePath);
                if (AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                {
                    var fileName = Path.GetFileName(filePath);
                    var request = HttpContext.Request;
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    var url = $"{baseUrl}/uploads/{selectedFolder}/{fileName}";
                    var fi = new FileInfo(filePath);
                    files.Add(new GalleryFileInfo(fileName, url, fi.Length, fi.LastWriteTime));
                }
            }
            files = files.OrderByDescending(f => f.LastModified).ToList();
        }

        ViewData["SelectedFolder"] = selectedFolder;
        ViewData["Folders"] = GalleryFolders;
        ViewData["Files"] = files;
        return View();
    }

    [Route("derin/api/gallery-files")]
    [HttpGet]
    [ResponseCache(NoStore = true, Duration = 0)]
    public IActionResult GetFiles([FromQuery(Name = "folder")] string? folder = "phones")
    {
        var folderLower = (folder ?? "").ToLowerInvariant();
        var selectedFolder = GalleryFolders.Contains(folderLower) ? folderLower : "phones";
        _logger.LogDebug("GetFiles: raw folder={Folder}, selectedFolder={SelectedFolder}", folder, selectedFolder);
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", selectedFolder);

        var files = new List<object>();
        if (Directory.Exists(uploadsPath))
        {
            foreach (var filePath in Directory.GetFiles(uploadsPath))
            {
                var ext = Path.GetExtension(filePath);
                if (AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                {
                    var fileName = Path.GetFileName(filePath);
                    var request = HttpContext.Request;
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    var url = $"{baseUrl}/uploads/{selectedFolder}/{fileName}";
                    var fi = new FileInfo(filePath);
                    files.Add(new { fileName, url });
                }
            }
        }
        return Json(new { folder = selectedFolder, files });
    }
}

public record GalleryFileInfo(string FileName, string Url, long SizeBytes, DateTime LastModified);
