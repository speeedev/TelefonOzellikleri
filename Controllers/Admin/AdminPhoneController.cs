using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;
using TelefonOzellikleri.Models.Dtos;
using TelefonOzellikleri.Models.Enums;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminPhoneController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminPhoneController> _logger;
        private readonly IMemoryCache _cache;

        public AdminPhoneController(TelefonOzellikleriDbContext context, ILogger<AdminPhoneController> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
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
                    s.Chipset
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

            var slugAvailable = await _context.IsSlugAvailableAsync(model.Slug);
            if (!slugAvailable)
            {
                ViewData["Title"] = "New Phone";
                ViewData["IsNew"] = true;
                ViewData["Error"] = "This slug is already used by a page, phone, or brand. Slugs must be unique across the site.";
                await PopulateDropdowns(model.BrandId, model.SeriesId);
                return View("Edit", model);
            }

            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            
            _context.Smartphones.Add(model);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Phone created: {Id} - {ModelName}", model.Id, model.ModelName);
            TempData["Success"] = $"{model.ModelName} created successfully.";
            return RedirectToAction("Edit", new { id = model.Id });
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

            var slugAvailable = await _context.IsSlugAvailableAsync(model.Slug, excludeSmartphoneId: id);
            if (!slugAvailable)
            {
                ViewData["Title"] = $"Edit: {phone.ModelName}";
                ViewData["Error"] = "This slug is already used by a page, phone, or brand. Slugs must be unique across the site.";
                await PopulateDropdowns(phone.BrandId, phone.SeriesId);
                return View("Edit", model);
            }

            phone.BrandId = model.BrandId;
            phone.SeriesId = model.SeriesId;
            phone.ModelName = model.ModelName;
            phone.Slug = model.Slug;
            phone.MainImageUrl = model.MainImageUrl;
            phone.ReleaseDate = model.ReleaseDate;
            phone.Status = model.Status;
            phone.OsType = model.OsType;
            phone.OsVersion = model.OsVersion;
            phone.UpdateGuaranteeYears = model.UpdateGuaranteeYears;
            phone.UpdateGuaranteeVersion = model.UpdateGuaranteeVersion;

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
            phone.UpdatedAt = DateTime.Now;
            ApplyListFields(phone);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Phone updated: {Id} - {ModelName}", phone.Id, phone.ModelName);

            if (!string.IsNullOrWhiteSpace(phone.Slug))
                _cache.Remove($"phone_detail_{phone.Slug.ToLowerInvariant()}");

            TempData["Success"] = $"{phone.ModelName} updated successfully.";
            return RedirectToAction("Edit", new { id = phone.Id });
        }

        [Route("derin/phones/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _context.Smartphones.FindAsync(id);
            if (phone == null)
                return NotFound();

            _context.Smartphones.Remove(phone);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Phone deleted: {Id} - {ModelName}", phone.Id, phone.ModelName);
            TempData["Success"] = $"{phone.ModelName} deleted successfully.";
            return RedirectToAction("Index");
        }

        [Route("derin/phones/export/{id}")]
        [HttpGet]
        public async Task<IActionResult> ExportJson(int id)
        {
            var phone = await _context.Smartphones.Include(p => p.Brand).Include(p => p.Series).FirstOrDefaultAsync(p => p.Id == id);
            if (phone == null)
                return NotFound();

            var dto = PhoneToDto(phone);
            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
            
            var fileName = $"{phone.Slug}.json";
            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", fileName);
        }

        [Route("derin/phones/import")]
        [HttpGet]
        public async Task<IActionResult> ImportJson()
        {
            ViewData["Title"] = "Import Phone from JSON";
            await PopulateDropdowns();
            return View();
        }

        [Route("derin/phones/import")]
        [HttpPost]
        public async Task<IActionResult> ImportJson(IFormFile jsonFile)
        {
            if (jsonFile == null || jsonFile.Length == 0)
            {
                ViewData["Error"] = "Please select a JSON file.";
                await PopulateDropdowns();
                return View();
            }

            try
            {
                using (var stream = new StreamReader(jsonFile.OpenReadStream()))
                {
                    var json = await stream.ReadToEndAsync();
                    var dto = JsonSerializer.Deserialize<SmartphoneJsonDto>(json);
                    
                    if (dto == null)
                    {
                        ViewData["Error"] = "Invalid JSON format.";
                        await PopulateDropdowns();
                        return View();
                    }

                    var phone = DtoToPhone(dto);
                    phone.CreatedAt = DateTime.Now;
                    phone.UpdatedAt = DateTime.Now;
                    _context.Smartphones.Add(phone);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Phone imported from JSON: {Id} - {ModelName}", phone.Id, phone.ModelName);
                    TempData["Success"] = $"{phone.ModelName} imported successfully from JSON.";
                    return RedirectToAction("Index");
                }
            }
            catch (JsonException ex)
            {
                ViewData["Error"] = $"JSON parsing error: {ex.Message}";
                await PopulateDropdowns();
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"Error importing phone: {ex.Message}";
                await PopulateDropdowns();
                return View();
            }
        }

        private SmartphoneJsonDto PhoneToDto(Smartphone phone)
        {
            return new SmartphoneJsonDto
            {
                General = new SmartphoneJsonDto.GeneralInfo
                {
                    BrandId = phone.BrandId,
                    BrandName = phone.Brand?.Name,
                    SeriesId = phone.SeriesId,
                    SeriesName = phone.Series?.SeriesName,
                    ModelName = phone.ModelName,
                    Slug = phone.Slug,
                    ReleaseDate = phone.ReleaseDate?.ToString("yyyy-MM-dd"),
                    Status = phone.Status?.ToString(),
                    MainImageUrl = phone.MainImageUrl,
                    Colors = phone.Colors,
                    BoxContents = phone.BoxContents
                },
                Display = new SmartphoneJsonDto.DisplayInfo
                {
                    ScreenSize = phone.ScreenSize,
                    ScreenTech = phone.ScreenTech,
                    ScreenRes = phone.ScreenRes,
                    RefreshRate = phone.RefreshRate,
                    PixelDensity = phone.PixelDensity,
                    ScreenBodyRatio = phone.ScreenBodyRatio,
                    ScreenBrightnessNits = phone.ScreenBrightnessNits,
                    ScreenAspectRatio = phone.ScreenAspectRatio,
                    ScreenProtection = phone.ScreenProtection,
                    ScreenExtraFeatures = phone.ScreenExtraFeatures
                },
                Design = new SmartphoneJsonDto.DesignInfo
                {
                    Height = phone.Height,
                    Width = phone.Width,
                    Thickness = phone.Thickness,
                    Weight = phone.Weight,
                    FrameMaterial = phone.FrameMaterial,
                    BackMaterial = phone.BackMaterial,
                    DustWaterRes = phone.DustWaterRes
                },
                Camera = new SmartphoneJsonDto.CameraInfo
                {
                    Rear = new SmartphoneJsonDto.RearCameraInfo
                    {
                        Main = phone.Cam1Exists == true ? new SmartphoneJsonDto.CameraSpec
                        {
                            Resolution = phone.Cam1Res,
                            Aperture = phone.Cam1Aperture,
                            Focal = phone.Cam1Focal,
                            SensorSize = phone.Cam1SensorSize,
                            PixelSize = phone.Cam1PixelSize,
                            Features = phone.Cam1Features
                        } : null,
                        Secondary = phone.Cam2Exists == true ? new SmartphoneJsonDto.CameraSpec
                        {
                            Type = phone.Cam2Type,
                            Resolution = phone.Cam2Res,
                            Aperture = phone.Cam2Aperture,
                            Focal = phone.Cam2Focal,
                            SensorSize = phone.Cam2SensorSize,
                            PixelSize = phone.Cam2PixelSize,
                            Features = phone.Cam2Features
                        } : null,
                        Tertiary = phone.Cam3Exists == true ? new SmartphoneJsonDto.CameraSpec
                        {
                            Type = phone.Cam3Type,
                            Resolution = phone.Cam3Res,
                            Aperture = phone.Cam3Aperture,
                            Focal = phone.Cam3Focal,
                            SensorSize = phone.Cam3SensorSize,
                            PixelSize = phone.Cam3PixelSize,
                            Features = phone.Cam3Features
                        } : null,
                        Quaternary = phone.Cam4Exists == true ? new SmartphoneJsonDto.CameraSpec
                        {
                            Type = phone.Cam4Type,
                            Resolution = phone.Cam4Res,
                            Aperture = phone.Cam4Aperture,
                            Focal = phone.Cam4Focal,
                            SensorSize = phone.Cam4SensorSize,
                            PixelSize = phone.Cam4PixelSize,
                            Features = phone.Cam4Features
                        } : null,
                        VideoRes = phone.RearVideoRes
                    },
                    Front = new SmartphoneJsonDto.FrontCameraInfo
                    {
                        FrontCamRes = phone.FrontCamRes,
                        FrontCamAperture = phone.FrontCamAperture,
                        FrontCamFocal = phone.FrontCamFocal,
                        FrontCamSensorSize = phone.FrontCamSensorSize,
                        FrontCamPixelSize = phone.FrontCamPixelSize,
                        FrontCamFeatures = phone.FrontCamFeatures,
                        FrontVideoRes = phone.FrontVideoRes,
                        SecondFrontRes = phone.SecondFrontRes
                    }
                },
                Hardware = new SmartphoneJsonDto.HardwareInfo
                {
                    Chipset = phone.Chipset,
                    Cpu = phone.Cpu,
                    Gpu = phone.Gpu,
                    AntutuScore = phone.AntutuScore,
                    GeekbenchScore = phone.GeekbenchScore,
                    RamOptions = phone.RamOptions,
                    StorageOptions = phone.StorageOptions,
                    StorageType = phone.StorageType,
                    SensorsList = phone.SensorsList
                },
                Battery = new SmartphoneJsonDto.BatteryInfo
                {
                    BatteryType = phone.BatteryType?.ToString(),
                    BatteryCapacity = phone.BatteryCapacity,
                    ChargingSpeed = phone.ChargingSpeed,
                    WirelessCharging = phone.WirelessCharging,
                    WirelessSpeed = phone.WirelessSpeed,
                    ReverseWireless = phone.ReverseWireless,
                    ReverseSpeed = phone.ReverseSpeed
                },
                Software = new SmartphoneJsonDto.SoftwareInfo
                {
                    OsType = phone.OsType?.ToString(),
                    OsVersion = phone.OsVersion,
                    UpdateGuaranteeYears = phone.UpdateGuaranteeYears,
                    UpdateGuaranteeVersion = phone.UpdateGuaranteeVersion,
                    HasAiFeatures = phone.HasAiFeatures,
                    AiFeaturesList = phone.AiFeaturesList
                },
                Connectivity = new SmartphoneJsonDto.ConnectivityInfo
                {
                    WifiVersion = phone.WifiVersion?.ToString(),
                    BluetoothVer = phone.BluetoothVer,
                    Nfc = phone.Nfc,
                    UsbType = phone.UsbType,
                    UsbVersion = phone.UsbVersion,
                    Support5g = phone.Support5g,
                    Support4g = phone.Support4g,
                    Support45g = phone.Support45g,
                    Gps = phone.Gps,
                    IrBlaster = phone.IrBlaster,
                    HasUwb = phone.HasUwb,
                    HasSatelliteSos = phone.HasSatelliteSos,
                    EsimSupport = phone.EsimSupport,
                    PhysicalSimCount = phone.PhysicalSimCount,
                    SpeakerType = phone.SpeakerType?.ToString()
                },
                Security = new SmartphoneJsonDto.SecurityInfo
                {
                    HasFaceRecognition = phone.HasFaceRecognition,
                    HasFingerprint = phone.HasFingerprint,
                    FingerprintType = phone.FingerprintType
                },
                Other = new SmartphoneJsonDto.OtherInfo
                {
                    SarHead = phone.SarHead,
                    SarBody = phone.SarBody
                }
            };
        }

        private Smartphone DtoToPhone(SmartphoneJsonDto dto)
        {
            var phone = new Smartphone
            {
                BrandId = dto.General?.BrandId ?? 1,
                SeriesId = dto.General?.SeriesId,
                ModelName = dto.General?.ModelName ?? "Unknown",
                Slug = dto.General?.Slug ?? Guid.NewGuid().ToString().Substring(0, 8),
                ReleaseDate = DateOnly.TryParse(dto.General?.ReleaseDate, out var date) ? date : null,
                Status = !string.IsNullOrEmpty(dto.General?.Status) ? Enum.Parse<PhoneStatus>(dto.General.Status, ignoreCase: true) : null,
                MainImageUrl = dto.General?.MainImageUrl,
                Colors = dto.General?.Colors,
                BoxContents = dto.General?.BoxContents,
                
                ScreenSize = dto.Display?.ScreenSize,
                ScreenTech = dto.Display?.ScreenTech,
                ScreenRes = dto.Display?.ScreenRes,
                RefreshRate = dto.Display?.RefreshRate,
                PixelDensity = dto.Display?.PixelDensity,
                ScreenBodyRatio = dto.Display?.ScreenBodyRatio,
                ScreenBrightnessNits = dto.Display?.ScreenBrightnessNits,
                ScreenAspectRatio = dto.Display?.ScreenAspectRatio,
                ScreenProtection = dto.Display?.ScreenProtection,
                ScreenExtraFeatures = dto.Display?.ScreenExtraFeatures,
                
                Height = dto.Design?.Height,
                Width = dto.Design?.Width,
                Thickness = dto.Design?.Thickness,
                Weight = dto.Design?.Weight,
                FrameMaterial = dto.Design?.FrameMaterial,
                BackMaterial = dto.Design?.BackMaterial,
                DustWaterRes = dto.Design?.DustWaterRes,
                
                Cam1Exists = dto.Camera?.Rear?.Main != null,
                Cam1Res = dto.Camera?.Rear?.Main?.Resolution,
                Cam1Aperture = dto.Camera?.Rear?.Main?.Aperture,
                Cam1Focal = dto.Camera?.Rear?.Main?.Focal,
                Cam1SensorSize = dto.Camera?.Rear?.Main?.SensorSize,
                Cam1PixelSize = dto.Camera?.Rear?.Main?.PixelSize,
                Cam1Features = dto.Camera?.Rear?.Main?.Features,
                
                Cam2Exists = dto.Camera?.Rear?.Secondary != null,
                Cam2Type = dto.Camera?.Rear?.Secondary?.Type,
                Cam2Res = dto.Camera?.Rear?.Secondary?.Resolution,
                Cam2Aperture = dto.Camera?.Rear?.Secondary?.Aperture,
                Cam2Focal = dto.Camera?.Rear?.Secondary?.Focal,
                Cam2SensorSize = dto.Camera?.Rear?.Secondary?.SensorSize,
                Cam2PixelSize = dto.Camera?.Rear?.Secondary?.PixelSize,
                Cam2Features = dto.Camera?.Rear?.Secondary?.Features,
                
                Cam3Exists = dto.Camera?.Rear?.Tertiary != null,
                Cam3Type = dto.Camera?.Rear?.Tertiary?.Type,
                Cam3Res = dto.Camera?.Rear?.Tertiary?.Resolution,
                Cam3Aperture = dto.Camera?.Rear?.Tertiary?.Aperture,
                Cam3Focal = dto.Camera?.Rear?.Tertiary?.Focal,
                Cam3SensorSize = dto.Camera?.Rear?.Tertiary?.SensorSize,
                Cam3PixelSize = dto.Camera?.Rear?.Tertiary?.PixelSize,
                Cam3Features = dto.Camera?.Rear?.Tertiary?.Features,
                
                Cam4Exists = dto.Camera?.Rear?.Quaternary != null,
                Cam4Type = dto.Camera?.Rear?.Quaternary?.Type,
                Cam4Res = dto.Camera?.Rear?.Quaternary?.Resolution,
                Cam4Aperture = dto.Camera?.Rear?.Quaternary?.Aperture,
                Cam4Focal = dto.Camera?.Rear?.Quaternary?.Focal,
                Cam4SensorSize = dto.Camera?.Rear?.Quaternary?.SensorSize,
                Cam4PixelSize = dto.Camera?.Rear?.Quaternary?.PixelSize,
                Cam4Features = dto.Camera?.Rear?.Quaternary?.Features,
                
                RearVideoRes = dto.Camera?.Rear?.VideoRes,
                
                FrontExists = dto.Camera?.Front?.FrontCamRes != null,
                FrontCamRes = dto.Camera?.Front?.FrontCamRes,
                FrontCamAperture = dto.Camera?.Front?.FrontCamAperture,
                FrontCamFocal = dto.Camera?.Front?.FrontCamFocal,
                FrontCamSensorSize = dto.Camera?.Front?.FrontCamSensorSize,
                FrontCamPixelSize = dto.Camera?.Front?.FrontCamPixelSize,
                FrontCamFeatures = dto.Camera?.Front?.FrontCamFeatures,
                FrontVideoRes = dto.Camera?.Front?.FrontVideoRes,
                SecondFrontRes = dto.Camera?.Front?.SecondFrontRes,
                
                Chipset = dto.Hardware?.Chipset,
                Cpu = dto.Hardware?.Cpu,
                Gpu = dto.Hardware?.Gpu,
                AntutuScore = dto.Hardware?.AntutuScore,
                GeekbenchScore = dto.Hardware?.GeekbenchScore,
                RamOptions = dto.Hardware?.RamOptions,
                StorageOptions = dto.Hardware?.StorageOptions,
                StorageType = dto.Hardware?.StorageType,
                SensorsList = dto.Hardware?.SensorsList,
                
                BatteryType = !string.IsNullOrEmpty(dto.Battery?.BatteryType) ? Enum.Parse<BatteryType>(dto.Battery.BatteryType, ignoreCase: true) : null,
                BatteryCapacity = dto.Battery?.BatteryCapacity,
                ChargingSpeed = dto.Battery?.ChargingSpeed,
                WirelessCharging = dto.Battery?.WirelessCharging,
                WirelessSpeed = dto.Battery?.WirelessSpeed,
                ReverseWireless = dto.Battery?.ReverseWireless,
                ReverseSpeed = dto.Battery?.ReverseSpeed,
                
                OsType = !string.IsNullOrEmpty(dto.Software?.OsType) ? Enum.Parse<OsType>(dto.Software.OsType, ignoreCase: true) : null,
                OsVersion = dto.Software?.OsVersion,
                UpdateGuaranteeYears = dto.Software?.UpdateGuaranteeYears,
                UpdateGuaranteeVersion = dto.Software?.UpdateGuaranteeVersion,
                HasAiFeatures = dto.Software?.HasAiFeatures,
                AiFeaturesList = dto.Software?.AiFeaturesList,
                
                WifiVersion = !string.IsNullOrEmpty(dto.Connectivity?.WifiVersion) ? Enum.Parse<Models.Enums.WifiVersion>(dto.Connectivity.WifiVersion, ignoreCase: true) : null,
                BluetoothVer = dto.Connectivity?.BluetoothVer,
                Nfc = dto.Connectivity?.Nfc,
                UsbType = dto.Connectivity?.UsbType,
                UsbVersion = dto.Connectivity?.UsbVersion,
                Support5g = dto.Connectivity?.Support5g,
                Support4g = dto.Connectivity?.Support4g,
                Support45g = dto.Connectivity?.Support45g,
                Gps = dto.Connectivity?.Gps,
                IrBlaster = dto.Connectivity?.IrBlaster,
                HasUwb = dto.Connectivity?.HasUwb,
                HasSatelliteSos = dto.Connectivity?.HasSatelliteSos,
                EsimSupport = dto.Connectivity?.EsimSupport,
                PhysicalSimCount = dto.Connectivity?.PhysicalSimCount,
                SpeakerType = dto.Connectivity?.SpeakerType != null ? Enum.Parse<Models.Enums.SpeakerType>(dto.Connectivity.SpeakerType, ignoreCase: true) : null,
                
                HasFaceRecognition = dto.Security?.HasFaceRecognition,
                HasFingerprint = dto.Security?.HasFingerprint,
                FingerprintType = dto.Security?.FingerprintType,
                
                SarHead = dto.Other?.SarHead,
                SarBody = dto.Other?.SarBody
            };

            return phone;
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
