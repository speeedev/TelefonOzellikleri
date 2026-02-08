using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminPhoneController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminPhoneController> _logger;

        public AdminPhoneController(TelefonOzellikleriDbContext context, ILogger<AdminPhoneController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("derin/phones")]
        public async Task<IActionResult> Index(string? search, int? brandId)
        {
            ViewData["Title"] = "Phones";

            var query = _context.Smartphones.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(s => s.ModelName.Contains(search) || s.Slug.Contains(search));

            if (brandId.HasValue)
                query = query.Where(s => s.BrandId == brandId.Value);

            var phones = await query
                .OrderByDescending(s => s.Id)
                .Select(s => new
                {
                    s.Id,
                    s.ModelName,
                    s.Slug,
                    s.MainImageUrl,
                    s.BrandId,
                    s.ScreenSize,
                    s.BatteryCapacity,
                    s.Chipset,
                    s.ReleaseDate
                })
                .ToListAsync();

            var brands = await _context.Brands.OrderBy(b => b.Name).ToListAsync();
            var brandDict = brands.ToDictionary(b => b.Id, b => b.Name);

            ViewData["Phones"] = phones;
            ViewData["Brands"] = brands;
            ViewData["BrandDict"] = brandDict;
            ViewData["Search"] = search;
            ViewData["BrandId"] = brandId;

            return View();
        }

        [Route("derin/phones/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "New Phone";
            ViewData["IsNew"] = true;
            await PopulateDropdowns();
            return View("Edit", new Smartphone());
        }

        [Route("derin/phones/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Smartphone model)
        {
            ApplyListFields(model);

            if (string.IsNullOrWhiteSpace(model.ModelName) || string.IsNullOrWhiteSpace(model.Slug))
            {
                ViewData["Title"] = "New Phone";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "Model Name and Slug are required.";
                await PopulateDropdowns(model.BrandId, model.SeriesId);
                return View("Edit", model);
            }

            var slugExists = await _context.Smartphones.AnyAsync(s => s.Slug == model.Slug);
            if (slugExists)
            {
                ViewData["Title"] = "New Phone";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "A phone with this slug already exists.";
                await PopulateDropdowns(model.BrandId, model.SeriesId);
                return View("Edit", model);
            }

            _context.Smartphones.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Phone created: {Id} - {ModelName}", model.Id, model.ModelName);
            TempData["Success"] = $"{model.ModelName} created successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/phones/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var phone = await _context.Smartphones.FindAsync(id);
            if (phone == null)
                return NotFound();

            ViewData["Title"] = $"Edit: {phone.ModelName}";
            await PopulateDropdowns(phone.BrandId, phone.SeriesId);

            return View(phone);
        }

        [Route("derin/phones/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Smartphone model)
        {
            if (id != model.Id)
                return BadRequest();

            var phone = await _context.Smartphones.FindAsync(id);
            if (phone == null)
                return NotFound();

            phone.BrandId = model.BrandId;
            phone.SeriesId = model.SeriesId;
            phone.ModelName = model.ModelName;
            phone.Slug = model.Slug;
            phone.MainImageUrl = model.MainImageUrl;
            phone.ReleaseDate = model.ReleaseDate;
            phone.PhoneStatus = model.PhoneStatus;
            phone.OsType = model.OsType;
            phone.OsVersion = model.OsVersion;
            phone.UpdateGuarantee = model.UpdateGuarantee;

            phone.Height = model.Height;
            phone.Width = model.Width;
            phone.Thickness = model.Thickness;
            phone.Weight = model.Weight;
            phone.FrameMaterial = model.FrameMaterial;
            phone.BackMaterial = model.BackMaterial;
            phone.ScreenProtection = model.ScreenProtection;
            phone.DustWaterRes = model.DustWaterRes;

            phone.ScreenSize = model.ScreenSize;
            phone.ScreenTech = model.ScreenTech;
            phone.ScreenRes = model.ScreenRes;
            phone.RefreshRate = model.RefreshRate;
            phone.PixelDensity = model.PixelDensity;
            phone.ScreenBodyRatio = model.ScreenBodyRatio;
            phone.ScreenAspectRatio = model.ScreenAspectRatio;
            phone.ScreenBrightnessNits = model.ScreenBrightnessNits;

            phone.Cam1Exists = model.Cam1Exists;
            phone.Cam1Res = model.Cam1Res;
            phone.Cam1Aperture = model.Cam1Aperture;
            phone.Cam1Focal = model.Cam1Focal;
            phone.Cam1Features = model.Cam1Features;
            phone.Cam1SensorSize = model.Cam1SensorSize;
            phone.Cam1PixelSize = model.Cam1PixelSize;
            phone.Cam2Exists = model.Cam2Exists;
            phone.Cam2Type = model.Cam2Type;
            phone.Cam2Res = model.Cam2Res;
            phone.Cam2Aperture = model.Cam2Aperture;
            phone.Cam2Focal = model.Cam2Focal;
            phone.Cam2Features = model.Cam2Features;
            phone.Cam2SensorSize = model.Cam2SensorSize;
            phone.Cam2PixelSize = model.Cam2PixelSize;
            phone.Cam3Exists = model.Cam3Exists;
            phone.Cam3Type = model.Cam3Type;
            phone.Cam3Res = model.Cam3Res;
            phone.Cam3Aperture = model.Cam3Aperture;
            phone.Cam3Focal = model.Cam3Focal;
            phone.Cam3Features = model.Cam3Features;
            phone.Cam3SensorSize = model.Cam3SensorSize;
            phone.Cam3PixelSize = model.Cam3PixelSize;
            phone.Cam4Exists = model.Cam4Exists;
            phone.Cam4Type = model.Cam4Type;
            phone.Cam4Res = model.Cam4Res;
            phone.Cam4Aperture = model.Cam4Aperture;
            phone.Cam4Focal = model.Cam4Focal;
            phone.Cam4Features = model.Cam4Features;
            phone.Cam4SensorSize = model.Cam4SensorSize;
            phone.Cam4PixelSize = model.Cam4PixelSize;
            phone.RearVideoRes = model.RearVideoRes;

            phone.FrontExists = model.FrontExists;
            phone.FrontCamRes = model.FrontCamRes;
            phone.FrontCamAperture = model.FrontCamAperture;
            phone.FrontCamFocal = model.FrontCamFocal;
            phone.FrontCamFeatures = model.FrontCamFeatures;
            phone.FrontCamSensorSize = model.FrontCamSensorSize;
            phone.FrontCamPixelSize = model.FrontCamPixelSize;
            phone.FrontVideoRes = model.FrontVideoRes;
            phone.SecondFrontExists = model.SecondFrontExists;
            phone.SecondFrontRes = model.SecondFrontRes;
            phone.SecondFrontSensorSize = model.SecondFrontSensorSize;
            phone.SecondFrontPixelSize = model.SecondFrontPixelSize;

            phone.HasFaceRecognition = model.HasFaceRecognition;
            phone.HasFingerprint = model.HasFingerprint;
            phone.FingerprintType = model.FingerprintType;

            phone.Chipset = model.Chipset;
            phone.Cpu = model.Cpu;
            phone.Gpu = model.Gpu;
            phone.AntutuScore = model.AntutuScore;
            phone.GeekbenchScore = model.GeekbenchScore;
            phone.StorageType = model.StorageType;
            phone.SensorsList = model.SensorsList;

            phone.HasAiFeatures = model.HasAiFeatures;
            phone.AiFeaturesList = model.AiFeaturesList;

            phone.BatteryType = model.BatteryType;
            phone.BatteryCapacity = model.BatteryCapacity;
            phone.ChargingSpeed = model.ChargingSpeed;
            phone.WirelessCharging = model.WirelessCharging;
            phone.WirelessSpeed = model.WirelessSpeed;
            phone.ReverseWireless = model.ReverseWireless;
            phone.ReverseSpeed = model.ReverseSpeed;

            phone.Support4g = model.Support4g;
            phone.Support45g = model.Support45g;
            phone.Support5g = model.Support5g;
            phone.EsimSupport = model.EsimSupport;
            phone.PhysicalSimCount = model.PhysicalSimCount;

            phone.WifiVersion = model.WifiVersion;
            phone.Nfc = model.Nfc;
            phone.BluetoothVer = model.BluetoothVer;
            phone.UsbType = model.UsbType;
            phone.UsbVersion = model.UsbVersion;
            phone.IrBlaster = model.IrBlaster;
            phone.Gps = model.Gps;
            phone.HasUwb = model.HasUwb;
            phone.HasSatelliteSos = model.HasSatelliteSos;
            phone.HeadphoneJack35mm = model.HeadphoneJack35mm;
            phone.SpeakerType = model.SpeakerType;

            phone.BoxContents = model.BoxContents;
            phone.SarHead = model.SarHead;
            phone.SarBody = model.SarBody;
            ApplyListFields(phone);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Phone updated: {Id} - {ModelName}", phone.Id, phone.ModelName);

            TempData["Success"] = $"{phone.ModelName} updated successfully.";
            return RedirectToAction("Index");
        }

        private void ApplyListFields(Smartphone phone)
        {
            phone.Colors = ParseCommaSeparated(Request.Form["ColorsRaw"]);
            phone.ScreenExtraFeatures = ParseCommaSeparated(Request.Form["ScreenExtraFeaturesRaw"]);
            phone.RamOptions = ParseCommaSeparated(Request.Form["RamOptionsRaw"]);
            phone.StorageOptions = ParseCommaSeparated(Request.Form["StorageOptionsRaw"]);
        }

        private static List<string>? ParseCommaSeparated(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();
        }

        private async Task PopulateDropdowns(int? selectedBrandId = null, int? selectedSeriesId = null)
        {
            var brands = await _context.Brands.OrderBy(b => b.Name).ToListAsync();
            var series = await _context.Series.OrderBy(s => s.SeriesName).ToListAsync();

            ViewData["BrandList"] = new SelectList(brands, "Id", "Name", selectedBrandId);
            ViewData["SeriesList"] = new SelectList(series, "Id", "SeriesName", selectedSeriesId);
        }
    }
}
