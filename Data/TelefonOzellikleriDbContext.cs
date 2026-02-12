using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Models;
using TelefonOzellikleri.Models.Enums;

namespace TelefonOzellikleri.Data;

public partial class TelefonOzellikleriDbContext : DbContext
{
    public TelefonOzellikleriDbContext(DbContextOptions<TelefonOzellikleriDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<SiteSetting> SiteSettings { get; set; }

    public virtual DbSet<Smartphone> Smartphones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("brands");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(255)
                .HasColumnName("logo_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pages");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.PageDescription)
                .HasMaxLength(255)
                .HasColumnName("page_description");
            entity.Property(e => e.PageTitle)
                .HasMaxLength(150)
                .HasColumnName("page_title");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("series");

            entity.HasIndex(e => e.BrandId, "fk_series_brand");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.SeriesName)
                .HasMaxLength(100)
                .HasColumnName("series_name");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasColumnName("slug");

            entity.HasOne(d => d.Brand).WithMany(p => p.Series)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_series_brand");
        });

        modelBuilder.Entity<SiteSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("site_settings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FaviconUrl)
                .HasMaxLength(255)
                .HasColumnName("favicon_url");
            entity.Property(e => e.FooterText)
                .HasColumnName("footer_text");
            entity.Property(e => e.InstagramUrl)
                .HasMaxLength(255)
                .HasColumnName("instagram_url");
            entity.Property(e => e.IsMaintenanceMode).HasColumnName("is_maintenance_mode");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(255)
                .HasColumnName("logo_url");
            entity.Property(e => e.SiteDescription)
                .HasMaxLength(255)
                .HasColumnName("site_description");
            entity.Property(e => e.SiteName)
                .HasMaxLength(255)
                .HasColumnName("site_name");
            entity.Property(e => e.SiteTitle)
                .HasMaxLength(255)
                .HasColumnName("site_title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.XUrl)
                .HasMaxLength(255)
                .HasColumnName("x_url");
            entity.Property(e => e.YoutubeUrl)
                .HasMaxLength(255)
                .HasColumnName("youtube_url");
        });

        modelBuilder.Entity<Smartphone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("smartphones")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.BrandId, "fk_brand");

            entity.HasIndex(e => e.SeriesId, "fk_series");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AiFeaturesList)
                .HasColumnType("text")
                .HasColumnName("ai_features_list");
            entity.Property(e => e.AntutuScore).HasColumnName("antutu_score");
            entity.Property(e => e.BackMaterial)
                .HasMaxLength(100)
                .HasColumnName("back_material");
            entity.Property(e => e.BatteryCapacity).HasColumnName("battery_capacity");
            entity.Property(e => e.BatteryType)
                .HasColumnName("battery_type");
            entity.Property(e => e.BluetoothVer)
                .HasMaxLength(50)
                .HasColumnName("bluetooth_ver");
            entity.Property(e => e.BoxContents)
                .HasColumnType("text")
                .HasColumnName("box_contents");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Cam1Aperture)
                .HasMaxLength(20)
                .HasColumnName("cam1_aperture");
            entity.Property(e => e.Cam1Exists)
                .HasColumnName("cam1_exists");
            entity.Property(e => e.Cam1Features)
                .HasColumnType("text")
                .HasColumnName("cam1_features");
            entity.Property(e => e.Cam1Focal)
                .HasMaxLength(20)
                .HasColumnName("cam1_focal");
            entity.Property(e => e.Cam1PixelSize)
                .HasMaxLength(20)
                .HasColumnName("cam1_pixel_size");
            entity.Property(e => e.Cam1Res)
                .HasMaxLength(50)
                .HasColumnName("cam1_res");
            entity.Property(e => e.Cam1SensorSize)
                .HasMaxLength(20)
                .HasColumnName("cam1_sensor_size");
            entity.Property(e => e.Cam2Aperture)
                .HasMaxLength(20)
                .HasColumnName("cam2_aperture");
            entity.Property(e => e.Cam2Exists).HasColumnName("cam2_exists");
            entity.Property(e => e.Cam2Features)
                .HasColumnType("text")
                .HasColumnName("cam2_features");
            entity.Property(e => e.Cam2Focal)
                .HasMaxLength(20)
                .HasColumnName("cam2_focal");
            entity.Property(e => e.Cam2PixelSize)
                .HasMaxLength(20)
                .HasColumnName("cam2_pixel_size");
            entity.Property(e => e.Cam2Res)
                .HasMaxLength(50)
                .HasColumnName("cam2_res");
            entity.Property(e => e.Cam2SensorSize)
                .HasMaxLength(20)
                .HasColumnName("cam2_sensor_size");
            entity.Property(e => e.Cam2Type)
                .HasMaxLength(50)
                .HasColumnName("cam2_type");
            entity.Property(e => e.Cam3Aperture)
                .HasMaxLength(20)
                .HasColumnName("cam3_aperture");
            entity.Property(e => e.Cam3Exists).HasColumnName("cam3_exists");
            entity.Property(e => e.Cam3Features)
                .HasColumnType("text")
                .HasColumnName("cam3_features");
            entity.Property(e => e.Cam3Focal)
                .HasMaxLength(20)
                .HasColumnName("cam3_focal");
            entity.Property(e => e.Cam3PixelSize)
                .HasMaxLength(20)
                .HasColumnName("cam3_pixel_size");
            entity.Property(e => e.Cam3Res)
                .HasMaxLength(50)
                .HasColumnName("cam3_res");
            entity.Property(e => e.Cam3SensorSize)
                .HasMaxLength(20)
                .HasColumnName("cam3_sensor_size");
            entity.Property(e => e.Cam3Type)
                .HasMaxLength(50)
                .HasColumnName("cam3_type");
            entity.Property(e => e.Cam4Aperture)
                .HasMaxLength(20)
                .HasColumnName("cam4_aperture");
            entity.Property(e => e.Cam4Exists).HasColumnName("cam4_exists");
            entity.Property(e => e.Cam4Features)
                .HasColumnType("text")
                .HasColumnName("cam4_features");
            entity.Property(e => e.Cam4Focal)
                .HasMaxLength(20)
                .HasColumnName("cam4_focal");
            entity.Property(e => e.Cam4PixelSize)
                .HasMaxLength(20)
                .HasColumnName("cam4_pixel_size");
            entity.Property(e => e.Cam4Res)
                .HasMaxLength(50)
                .HasColumnName("cam4_res");
            entity.Property(e => e.Cam4SensorSize)
                .HasMaxLength(20)
                .HasColumnName("cam4_sensor_size");
            entity.Property(e => e.Cam4Type)
                .HasMaxLength(50)
                .HasColumnName("cam4_type");
            entity.Property(e => e.ChargingSpeed).HasColumnName("charging_speed");
            entity.Property(e => e.Chipset)
                .HasMaxLength(150)
                .HasColumnName("chipset");
            entity.Property(e => e.Colors)
                .HasColumnType("json")
                .HasColumnName("colors");
            entity.Property(e => e.Cpu)
                .HasMaxLength(150)
                .HasColumnName("cpu");
            entity.Property(e => e.DustWaterRes)
                .HasMaxLength(50)
                .HasColumnName("dust_water_res");
            entity.Property(e => e.EsimSupport).HasColumnName("esim_support");
            entity.Property(e => e.FingerprintType)
                .HasMaxLength(50)
                .HasColumnName("fingerprint_type");
            entity.Property(e => e.FrameMaterial)
                .HasMaxLength(100)
                .HasColumnName("frame_material");
            entity.Property(e => e.FrontCamAperture)
                .HasMaxLength(20)
                .HasColumnName("front_cam_aperture");
            entity.Property(e => e.FrontCamFeatures)
                .HasColumnType("text")
                .HasColumnName("front_cam_features");
            entity.Property(e => e.FrontCamFocal)
                .HasMaxLength(20)
                .HasColumnName("front_cam_focal");
            entity.Property(e => e.FrontCamPixelSize)
                .HasMaxLength(20)
                .HasColumnName("front_cam_pixel_size");
            entity.Property(e => e.FrontCamRes)
                .HasMaxLength(50)
                .HasColumnName("front_cam_res");
            entity.Property(e => e.FrontCamSensorSize)
                .HasMaxLength(20)
                .HasColumnName("front_cam_sensor_size");
            entity.Property(e => e.FrontExists)
                .HasColumnName("front_exists");
            entity.Property(e => e.FrontVideoRes)
                .HasMaxLength(100)
                .HasColumnName("front_video_res");
            entity.Property(e => e.GeekbenchScore)
                .HasMaxLength(100)
                .HasColumnName("geekbench_score");
            entity.Property(e => e.Gps)
                .HasColumnName("gps");
            entity.Property(e => e.Gpu)
                .HasMaxLength(150)
                .HasColumnName("gpu");
            entity.Property(e => e.HasAiFeatures).HasColumnName("has_ai_features");
            entity.Property(e => e.HasFaceRecognition).HasColumnName("has_face_recognition");
            entity.Property(e => e.HasFingerprint).HasColumnName("has_fingerprint");
            entity.Property(e => e.HasSatelliteSos).HasColumnName("has_satellite_sos");
            entity.Property(e => e.HasUwb).HasColumnName("has_uwb");
            entity.Property(e => e.HeadphoneJack35mm).HasColumnName("headphone_jack_35mm");
            entity.Property(e => e.Height)
                .HasPrecision(10, 2)
                .HasColumnName("height");
            entity.Property(e => e.IrBlaster).HasColumnName("ir_blaster");
            entity.Property(e => e.MainImageUrl)
                .HasMaxLength(255)
                .HasColumnName("main_image_url");
            entity.Property(e => e.ModelName)
                .HasMaxLength(255)
                .HasColumnName("model_name");
            entity.Property(e => e.Nfc).HasColumnName("nfc");
            entity.Property(e => e.OsType)
                .HasColumnName("os_type");
            entity.Property(e => e.OsVersion)
                .HasMaxLength(50)
                .HasColumnName("os_version");
            entity.Property(e => e.PhysicalSimCount)
                .HasDefaultValueSql("'1'")
                .HasColumnName("physical_sim_count");
            entity.Property(e => e.PixelDensity).HasColumnName("pixel_density");
            entity.Property(e => e.RamOptions)
                .HasColumnType("json")
                .HasColumnName("ram_options");
            entity.Property(e => e.RearVideoRes)
                .HasMaxLength(100)
                .HasColumnName("rear_video_res");
            entity.Property(e => e.RefreshRate).HasColumnName("refresh_rate");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.ReverseSpeed).HasColumnName("reverse_speed");
            entity.Property(e => e.ReverseWireless).HasColumnName("reverse_wireless");
            entity.Property(e => e.SarBody)
                .HasPrecision(5, 3)
                .HasColumnName("sar_body");
            entity.Property(e => e.SarHead)
                .HasPrecision(5, 3)
                .HasColumnName("sar_head");
            entity.Property(e => e.ScreenAspectRatio)
                .HasMaxLength(20)
                .HasColumnName("screen_aspect_ratio");
            entity.Property(e => e.ScreenBodyRatio)
                .HasPrecision(5, 2)
                .HasColumnName("screen_body_ratio");
            entity.Property(e => e.ScreenBrightnessNits).HasColumnName("screen_brightness_nits");
            entity.Property(e => e.ScreenExtraFeatures)
                .HasColumnType("json")
                .HasColumnName("screen_extra_features");
            entity.Property(e => e.ScreenProtection)
                .HasMaxLength(100)
                .HasColumnName("screen_protection");
            entity.Property(e => e.ScreenRes)
                .HasMaxLength(100)
                .HasColumnName("screen_res");
            entity.Property(e => e.ScreenSize)
                .HasPrecision(10, 2)
                .HasColumnName("screen_size");
            entity.Property(e => e.ScreenTech)
                .HasMaxLength(100)
                .HasColumnName("screen_tech");
            entity.Property(e => e.SecondFrontExists).HasColumnName("second_front_exists");
            entity.Property(e => e.SecondFrontPixelSize)
                .HasMaxLength(20)
                .HasColumnName("second_front_pixel_size");
            entity.Property(e => e.SecondFrontRes)
                .HasMaxLength(50)
                .HasColumnName("second_front_res");
            entity.Property(e => e.SecondFrontSensorSize)
                .HasMaxLength(20)
                .HasColumnName("second_front_sensor_size");
            entity.Property(e => e.SensorsList)
                .HasColumnType("text")
                .HasColumnName("sensors_list");
            entity.Property(e => e.SeriesId).HasColumnName("series_id");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.SpeakerType)
                .HasColumnName("speaker_type");
            entity.Property(e => e.Status)
                .HasDefaultValue(PhoneStatus.Unannounced)
                .HasColumnName("status");
            entity.Property(e => e.StorageOptions)
                .HasColumnType("json")
                .HasColumnName("storage_options");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");
            entity.Property(e => e.StorageType)
                .HasMaxLength(50)
                .HasColumnName("storage_type");
            entity.Property(e => e.Support45g).HasColumnName("support_4_5g");
            entity.Property(e => e.Support4g)
                .HasColumnName("support_4g");
            entity.Property(e => e.Support5g).HasColumnName("support_5g");
            entity.Property(e => e.Thickness)
                .HasPrecision(10, 2)
                .HasColumnName("thickness");
            entity.Property(e => e.UpdateGuaranteeYears)
                .HasMaxLength(50)
                .HasColumnName("update_guarantee");
            entity.Property(e => e.UpdateGuaranteeVersion)
                .HasMaxLength(50)
                .HasColumnName("update_guarantee_version");
            entity.Property(e => e.UsbType)
                .HasMaxLength(50)
                .HasColumnName("usb_type");
            entity.Property(e => e.UsbVersion)
                .HasMaxLength(50)
                .HasColumnName("usb_version");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
            entity.Property(e => e.Width)
                .HasPrecision(10, 2)
                .HasColumnName("width");
            entity.Property(e => e.WifiVersion)
                .HasColumnName("wifi_version");
            entity.Property(e => e.WirelessCharging).HasColumnName("wireless_charging");
            entity.Property(e => e.WirelessSpeed).HasColumnName("wireless_speed");

            entity.HasOne(d => d.Brand).WithMany(p => p.Smartphones)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_brand");

            entity.HasOne(d => d.Series).WithMany(p => p.Smartphones)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_series");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
