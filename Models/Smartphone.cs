using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class Smartphone
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public int? SeriesId { get; set; }

    public string ModelName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? MainImageUrl { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? OsVersion { get; set; }

    public string? UpdateGuarantee { get; set; }

    public decimal? Height { get; set; }

    public decimal? Width { get; set; }

    public decimal? Thickness { get; set; }

    public decimal? Weight { get; set; }

    public string? FrameMaterial { get; set; }

    public string? BackMaterial { get; set; }

    public string? ScreenProtection { get; set; }

    public string? DustWaterRes { get; set; }

    public decimal? ScreenSize { get; set; }

    public string? ScreenTech { get; set; }

    public string? ScreenRes { get; set; }

    public int? RefreshRate { get; set; }

    public int? PixelDensity { get; set; }

    public decimal? ScreenBodyRatio { get; set; }

    public int? ScreenBrightnessNits { get; set; }

    public List<string>? ScreenExtraFeatures { get; set; }

    public bool Cam1Exists { get; set; }

    public string? Cam1Res { get; set; }

    public string? Cam1Aperture { get; set; }

    public string? Cam1Focal { get; set; }

    public string? Cam1Features { get; set; }

    public bool Cam2Exists { get; set; }

    public string? Cam2Type { get; set; }

    public string? Cam2Res { get; set; }

    public string? Cam2Aperture { get; set; }

    public string? Cam2Focal { get; set; }

    public string? Cam2Features { get; set; }

    public bool Cam3Exists { get; set; }

    public string? Cam3Type { get; set; }

    public string? Cam3Res { get; set; }

    public string? Cam3Aperture { get; set; }

    public string? Cam3Focal { get; set; }

    public string? Cam3Features { get; set; }

    public bool Cam4Exists { get; set; }

    public string? Cam4Type { get; set; }

    public string? Cam4Res { get; set; }

    public string? Cam4Aperture { get; set; }

    public string? Cam4Focal { get; set; }

    public string? Cam4Features { get; set; }

    public string? RearVideoRes { get; set; }

    public bool FrontExists { get; set; }

    public string? FrontCamRes { get; set; }

    public string? FrontCamAperture { get; set; }

    public string? FrontCamFocal { get; set; }

    public string? FrontCamFeatures { get; set; }

    public string? FrontVideoRes { get; set; }

    public bool SecondFrontExists { get; set; }

    public string? SecondFrontRes { get; set; }

    public string? Chipset { get; set; }

    public string? Cpu { get; set; }

    public string? Gpu { get; set; }

    public int? AntutuScore { get; set; }

    public string? GeekbenchScore { get; set; }

    public int? BatteryCapacity { get; set; }

    public int? ChargingSpeed { get; set; }

    public bool WirelessCharging { get; set; }

    public int? WirelessSpeed { get; set; }

    public bool ReverseWireless { get; set; }

    public int? ReverseSpeed { get; set; }

    public bool HasAiFeatures { get; set; }

    public string? AiFeaturesList { get; set; }

    public string? SpeakerType { get; set; }

    public bool HeadphoneJack35mm { get; set; }

    public bool Nfc { get; set; }

    public string? BluetoothVer { get; set; }

    public bool IrBlaster { get; set; }

    public bool Gps { get; set; }

    public string? SensorsList { get; set; }

    public string? UsbType { get; set; }

    public string? UsbVersion { get; set; }

    public bool EsimSupport { get; set; }

    public short PhysicalSimCount { get; set; }

    public string? BoxContents { get; set; }

    public List<string>? Colors { get; set; }

    public bool Support5g { get; set; }

    public bool Support4g { get; set; }
}
