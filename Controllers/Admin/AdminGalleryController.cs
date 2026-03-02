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

    [Route("derin/api/gallery-delete")]
    [HttpPost]
    [ResponseCache(NoStore = true, Duration = 0)]
    public IActionResult DeleteFile([FromForm(Name = "folder")] string? folder, [FromForm(Name = "fileName")] string? fileName)
    {
        var folderLower = (folder ?? "").ToLowerInvariant();
        var selectedFolder = GalleryFolders.Contains(folderLower) ? folderLower : null;
        if (selectedFolder == null || string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { error = "Invalid folder or file name." });

        if (fileName.Contains("..", StringComparison.Ordinal) || Path.GetFileName(fileName) != fileName)
            return BadRequest(new { error = "Invalid file name." });

        var ext = Path.GetExtension(fileName);
        if (!AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { error = "Invalid file type." });

        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", selectedFolder);
        var filePath = Path.Combine(uploadsPath, Path.GetFileName(fileName));

        if (!System.IO.File.Exists(filePath))
            return NotFound(new { error = "File not found." });

        try
        {
            System.IO.File.Delete(filePath);
            _logger.LogInformation("Gallery file deleted: {Folder}/{FileName}", selectedFolder, fileName);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete gallery file: {Path}", filePath);
            return StatusCode(500, new { error = "Failed to delete file." });
        }
    }
}

public record GalleryFileInfo(string FileName, string Url, long SizeBytes, DateTime LastModified);
