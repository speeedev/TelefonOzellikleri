using System.Text.Json.Serialization;

namespace TelefonOzellikleri.Models.Dtos;

public class SmartphoneJsonDto
{
    [JsonPropertyName("general")]
    public GeneralInfo? General { get; set; }

    [JsonPropertyName("display")]
    public DisplayInfo? Display { get; set; }

    [JsonPropertyName("design")]
    public DesignInfo? Design { get; set; }

    [JsonPropertyName("camera")]
    public CameraInfo? Camera { get; set; }

    [JsonPropertyName("hardware")]
    public HardwareInfo? Hardware { get; set; }

    [JsonPropertyName("battery")]
    public BatteryInfo? Battery { get; set; }

    [JsonPropertyName("software")]
    public SoftwareInfo? Software { get; set; }

    [JsonPropertyName("connectivity")]
    public ConnectivityInfo? Connectivity { get; set; }

    [JsonPropertyName("security")]
    public SecurityInfo? Security { get; set; }

    [JsonPropertyName("other")]
    public OtherInfo? Other { get; set; }

    public class GeneralInfo
    {
        [JsonPropertyName("brandId")]
        public int? BrandId { get; set; }

        [JsonPropertyName("brand")]
        public string? BrandName { get; set; }

        [JsonPropertyName("seriesId")]
        public int? SeriesId { get; set; }

        [JsonPropertyName("series")]
        public string? SeriesName { get; set; }

        [JsonPropertyName("name")]
        public string? ModelName { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("releaseDate")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("mainImageUrl")]
        public string? MainImageUrl { get; set; }

        [JsonPropertyName("colors")]
        public List<string>? Colors { get; set; }

        [JsonPropertyName("boxContents")]
        public string? BoxContents { get; set; }
    }

    public class DisplayInfo
    {
        [JsonPropertyName("size")]
        public decimal? ScreenSize { get; set; }

        [JsonPropertyName("technology")]
        public string? ScreenTech { get; set; }

        [JsonPropertyName("resolution")]
        public string? ScreenRes { get; set; }

        [JsonPropertyName("refreshRate")]
        public int? RefreshRate { get; set; }

        [JsonPropertyName("pixelDensity")]
        public int? PixelDensity { get; set; }

        [JsonPropertyName("bodyRatio")]
        public decimal? ScreenBodyRatio { get; set; }

        [JsonPropertyName("brightness")]
        public int? ScreenBrightnessNits { get; set; }

        [JsonPropertyName("aspectRatio")]
        public string? ScreenAspectRatio { get; set; }

        [JsonPropertyName("protection")]
        public string? ScreenProtection { get; set; }

        [JsonPropertyName("extraFeatures")]
        public List<string>? ScreenExtraFeatures { get; set; }
    }

    public class DesignInfo
    {
        [JsonPropertyName("height")]
        public decimal? Height { get; set; }

        [JsonPropertyName("width")]
        public decimal? Width { get; set; }

        [JsonPropertyName("thickness")]
        public decimal? Thickness { get; set; }

        [JsonPropertyName("weight")]
        public decimal? Weight { get; set; }

        [JsonPropertyName("frameMaterial")]
        public string? FrameMaterial { get; set; }

        [JsonPropertyName("backMaterial")]
        public string? BackMaterial { get; set; }

        [JsonPropertyName("dustWaterResistance")]
        public string? DustWaterRes { get; set; }
    }

    public class CameraInfo
    {
        [JsonPropertyName("rear")]
        public RearCameraInfo? Rear { get; set; }

        [JsonPropertyName("front")]
        public FrontCameraInfo? Front { get; set; }
    }

    public class RearCameraInfo
    {
        [JsonPropertyName("main")]
        public CameraSpec? Main { get; set; }

        [JsonPropertyName("secondary")]
        public CameraSpec? Secondary { get; set; }

        [JsonPropertyName("tertiary")]
        public CameraSpec? Tertiary { get; set; }

        [JsonPropertyName("quaternary")]
        public CameraSpec? Quaternary { get; set; }

        [JsonPropertyName("videoResolution")]
        public string? VideoRes { get; set; }
    }

    public class FrontCameraInfo
    {
        [JsonPropertyName("resolution")]
        public string? FrontCamRes { get; set; }

        [JsonPropertyName("aperture")]
        public string? FrontCamAperture { get; set; }

        [JsonPropertyName("focal")]
        public string? FrontCamFocal { get; set; }

        [JsonPropertyName("sensorSize")]
        public string? FrontCamSensorSize { get; set; }

        [JsonPropertyName("pixelSize")]
        public string? FrontCamPixelSize { get; set; }

        [JsonPropertyName("features")]
        public string? FrontCamFeatures { get; set; }

        [JsonPropertyName("videoResolution")]
        public string? FrontVideoRes { get; set; }

        [JsonPropertyName("secondFrontResolution")]
        public string? SecondFrontRes { get; set; }
    }

    public class CameraSpec
    {
        [JsonPropertyName("resolution")]
        public string? Resolution { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("aperture")]
        public string? Aperture { get; set; }

        [JsonPropertyName("focal")]
        public string? Focal { get; set; }

        [JsonPropertyName("sensorSize")]
        public string? SensorSize { get; set; }

        [JsonPropertyName("pixelSize")]
        public string? PixelSize { get; set; }

        [JsonPropertyName("features")]
        public string? Features { get; set; }
    }

    public class HardwareInfo
    {
        [JsonPropertyName("chipset")]
        public string? Chipset { get; set; }

        [JsonPropertyName("cpu")]
        public string? Cpu { get; set; }

        [JsonPropertyName("gpu")]
        public string? Gpu { get; set; }

        [JsonPropertyName("antutuScore")]
        public int? AntutuScore { get; set; }

        [JsonPropertyName("geekbenchScore")]
        public string? GeekbenchScore { get; set; }

        [JsonPropertyName("ramOptions")]
        public List<string>? RamOptions { get; set; }

        [JsonPropertyName("storageOptions")]
        public List<string>? StorageOptions { get; set; }

        [JsonPropertyName("storageType")]
        public string? StorageType { get; set; }

        [JsonPropertyName("sensors")]
        public string? SensorsList { get; set; }
    }

    public class BatteryInfo
    {
        [JsonPropertyName("type")]
        public string? BatteryType { get; set; }

        [JsonPropertyName("capacity")]
        public int? BatteryCapacity { get; set; }

        [JsonPropertyName("chargingSpeed")]
        public int? ChargingSpeed { get; set; }

        [JsonPropertyName("wirelessCharging")]
        public bool? WirelessCharging { get; set; }

        [JsonPropertyName("wirelessSpeed")]
        public int? WirelessSpeed { get; set; }

        [JsonPropertyName("reverseWireless")]
        public bool? ReverseWireless { get; set; }

        [JsonPropertyName("reverseSpeed")]
        public int? ReverseSpeed { get; set; }
    }

    public class SoftwareInfo
    {
        [JsonPropertyName("osType")]
        public string? OsType { get; set; }

        [JsonPropertyName("osVersion")]
        public string? OsVersion { get; set; }

        [JsonPropertyName("updateGuarantee")]
        public string? UpdateGuarantee { get; set; }

        [JsonPropertyName("hasAiFeatures")]
        public bool? HasAiFeatures { get; set; }

        [JsonPropertyName("aiFeaturesList")]
        public string? AiFeaturesList { get; set; }
    }

    public class ConnectivityInfo
    {
        [JsonPropertyName("wifiVersion")]
        public string? WifiVersion { get; set; }

        [JsonPropertyName("bluetoothVersion")]
        public string? BluetoothVer { get; set; }

        [JsonPropertyName("nfc")]
        public bool? Nfc { get; set; }

        [JsonPropertyName("usbType")]
        public string? UsbType { get; set; }

        [JsonPropertyName("usbVersion")]
        public string? UsbVersion { get; set; }

        [JsonPropertyName("5g")]
        public bool? Support5g { get; set; }

        [JsonPropertyName("4g")]
        public bool? Support4g { get; set; }

        [JsonPropertyName("4_5g")]
        public bool? Support45g { get; set; }

        [JsonPropertyName("gps")]
        public bool? Gps { get; set; }

        [JsonPropertyName("irBlaster")]
        public bool? IrBlaster { get; set; }

        [JsonPropertyName("uwb")]
        public bool? HasUwb { get; set; }

        [JsonPropertyName("satelliteSos")]
        public bool? HasSatelliteSos { get; set; }

        [JsonPropertyName("esim")]
        public bool? EsimSupport { get; set; }

        [JsonPropertyName("physicalSimCount")]
        public short? PhysicalSimCount { get; set; }

        [JsonPropertyName("speakerType")]
        public string? SpeakerType { get; set; }
    }

    public class SecurityInfo
    {
        [JsonPropertyName("faceRecognition")]
        public bool? HasFaceRecognition { get; set; }

        [JsonPropertyName("fingerprint")]
        public bool? HasFingerprint { get; set; }

        [JsonPropertyName("fingerprintType")]
        public string? FingerprintType { get; set; }
    }

    public class OtherInfo
    {
        [JsonPropertyName("sarHead")]
        public decimal? SarHead { get; set; }

        [JsonPropertyName("sarBody")]
        public decimal? SarBody { get; set; }
    }
}
