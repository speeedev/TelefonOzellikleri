using System;
using System.Collections.Generic;
using TelefonOzellikleri.Models.Enums;

namespace TelefonOzellikleri.Models;

public partial class Smartphone
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public int? SeriesId { get; set; }

    public string ModelName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    public PhoneStatus? Status { get; set; }

    public OsType? OsType { get; set; }

    public WifiVersion? WifiVersion { get; set; }

    public BatteryType? BatteryType { get; set; }

    public string? OsVersion { get; set; }

    public string? UpdateGuaranteeYears { get; set; }

    public string? UpdateGuaranteeVersion { get; set; }

    public decimal? Height { get; set; }

    public decimal? Width { get; set; }

    public decimal? Thickness { get; set; }

    public decimal? Weight { get; set; }

    public bool? IsFoldable { get; set; }

    public decimal? HeightFolded { get; set; }

    public decimal? WidthFolded { get; set; }

    public decimal? ThicknessFolded { get; set; }

    public decimal? HeightUnfolded { get; set; }

    public decimal? WidthUnfolded { get; set; }

    public decimal? ThicknessUnfolded { get; set; }

    public string? FrameMaterial { get; set; }

    public string? BackMaterial { get; set; }

    public string? ScreenProtection { get; set; }

    public string? DustResistance { get; set; }

    public bool? DustResistanceExists { get; set; }

    public string? WaterResistance { get; set; }

    public bool? WaterResistanceExists { get; set; }

    public decimal? ScreenSize { get; set; }

    public string? ScreenTech { get; set; }

    public string? ScreenRes { get; set; }

    public int? RefreshRate { get; set; }

    public int? PixelDensity { get; set; }

    public decimal? ScreenBodyRatio { get; set; }

    public int? ScreenBrightnessNits { get; set; }

    public string? ScreenAspectRatio { get; set; }

    public int? ScreenTouchSamplingRate { get; set; }

    public string? ScreenOtherSpecs { get; set; }

    public bool? AlwaysOnDisplay { get; set; }

    public bool? Screen2Exists { get; set; }

    public decimal? Screen2Size { get; set; }

    public string? Screen2Tech { get; set; }

    public string? Screen2Res { get; set; }

    public int? Screen2RefreshRate { get; set; }

    public int? Screen2PixelDensity { get; set; }

    public string? Screen2AspectRatio { get; set; }

    public string? Screen2Protection { get; set; }

    public string? Screen2OtherSpecs { get; set; }

    public int? Screen2TouchSamplingRate { get; set; }

    public bool? Cam1Exists { get; set; }

    public string? Cam1Res { get; set; }

    public string? Cam1Aperture { get; set; }

    public string? Cam1Focal { get; set; }

    public string? Cam1SensorSize { get; set; }

    public string? Cam1PixelSize { get; set; }

    public string? Cam1Features { get; set; }

    public bool? Cam2Exists { get; set; }

    public string? Cam2Type { get; set; }

    public string? Cam2Res { get; set; }

    public string? Cam2Aperture { get; set; }

    public string? Cam2Focal { get; set; }

    public string? Cam2SensorSize { get; set; }

    public string? Cam2PixelSize { get; set; }

    public string? Cam2Features { get; set; }

    public bool? Cam3Exists { get; set; }

    public string? Cam3Type { get; set; }

    public string? Cam3Res { get; set; }

    public string? Cam3Aperture { get; set; }

    public string? Cam3Focal { get; set; }

    public string? Cam3SensorSize { get; set; }

    public string? Cam3PixelSize { get; set; }

    public string? Cam3Features { get; set; }

    public bool? Cam4Exists { get; set; }

    public string? Cam4Type { get; set; }

    public string? Cam4Res { get; set; }

    public string? Cam4Aperture { get; set; }

    public string? Cam4Focal { get; set; }

    public string? Cam4SensorSize { get; set; }

    public string? Cam4PixelSize { get; set; }

    public string? Cam4Features { get; set; }

    public string? RearVideoRes { get; set; }

    public bool? FrontExists { get; set; }

    public string? FrontCamRes { get; set; }

    public string? FrontCamAperture { get; set; }

    public string? FrontCamFocal { get; set; }

    public string? FrontCamSensorSize { get; set; }

    public string? FrontCamPixelSize { get; set; }

    public string? FrontCamFeatures { get; set; }

    public string? FrontVideoRes { get; set; }

    public bool? FrontCoverExists { get; set; }

    public string? FrontCoverRes { get; set; }

    public string? FrontCoverAperture { get; set; }

    public string? FrontCoverFocal { get; set; }

    public string? FrontCoverSensorSize { get; set; }

    public string? FrontCoverPixelSize { get; set; }

    public string? FrontCoverFeatures { get; set; }

    public string? FrontCoverVideoRes { get; set; }

    public bool? SecondFrontExists { get; set; }

    public string? SecondFrontRes { get; set; }

    public string? SecondFrontSensorSize { get; set; }

    public string? SecondFrontPixelSize { get; set; }

    public string? Chipset { get; set; }

    public string? Cpu { get; set; }

    public string? Gpu { get; set; }

    public int? AntutuScore { get; set; }

    public string? GeekbenchScore { get; set; }

    public string? StorageType { get; set; }

    public int? BatteryCapacity { get; set; }

    public int? ChargingSpeed { get; set; }

    public bool? WirelessCharging { get; set; }

    public int? WirelessSpeed { get; set; }

    public bool? ReverseWireless { get; set; }

    public int? ReverseWirelessSpeed { get; set; }

    public bool? ReverseWired { get; set; }

    public int? ReverseWiredSpeed { get; set; }

    public bool? HasAiFeatures { get; set; }

    public string? AiFeaturesList { get; set; }

    public SpeakerType? SpeakerType { get; set; }

    public bool? HeadphoneJack35mm { get; set; }

    public bool? Nfc { get; set; }

    public string? BluetoothVer { get; set; }

    public bool? IrBlaster { get; set; }

    public bool? Gps { get; set; }

    public string? SensorsList { get; set; }

    public string? UsbType { get; set; }

    public string? UsbVersion { get; set; }

    public bool? EsimSupport { get; set; }

    public short? PhysicalSimCount { get; set; }

    public string? BoxContents { get; set; }

    public bool? Support5g { get; set; }

    public bool? Support4g { get; set; }

    public bool? Support45g { get; set; }

    public bool? HasSatelliteSos { get; set; }

    public bool? HasUwb { get; set; }

    public bool? HasFaceRecognition { get; set; }

    public bool? HasFingerprint { get; set; }

    public string? FingerprintType { get; set; }

    public decimal? SarHead { get; set; }

    public decimal? SarBody { get; set; }

    public string? MainImageUrl { get; set; }

    public List<string>? ImageGallery { get; set; }

    public List<string>? Colors { get; set; }

    public List<string>? ScreenExtraFeatures { get; set; }

    public List<string>? RamOptions { get; set; }

    public List<string>? StorageOptions { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Series? Series { get; set; }
}
