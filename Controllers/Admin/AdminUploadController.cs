using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminUploadController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminUploadController> _logger;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg"
        };

        private const long MaxFileSize = 2 * 1024 * 1024;
        private const int MaxWidth = 800;
        private const int MaxHeight = 800;

        public AdminUploadController(IWebHostEnvironment env, ILogger<AdminUploadController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [Route("derin/upload/image")]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadImage(IFormFile file, string? folder)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file provided." });

            if (file.Length > MaxFileSize)
                return BadRequest(new { error = "File size exceeds 2 MB limit." });

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext))
                return BadRequest(new { error = "Invalid file type. Allowed: jpg, png, webp, gif, svg." });

            var subfolder = folder switch
            {
                "brands" => "brands",
                "pages" => "pages",
                _ => "phones"
            };

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}.webp";
            var filePath = Path.Combine(uploadsPath, fileName);

            if (ext.Equals(".svg", StringComparison.OrdinalIgnoreCase))
            {
                fileName = $"{Guid.NewGuid()}.svg";
                filePath = Path.Combine(uploadsPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            else
            {
                using var image = await Image.LoadAsync(file.OpenReadStream());

                if (image.Width > MaxWidth || image.Height > MaxHeight)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(MaxWidth, MaxHeight)
                    }));
                }

                await image.SaveAsWebpAsync(filePath);
            }

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var url = $"{baseUrl}/uploads/{subfolder}/{fileName}";
            _logger.LogInformation("Image uploaded: {Url} (original: {OriginalName})", url, file.FileName);

            return Ok(new { url });
        }
    }
}
