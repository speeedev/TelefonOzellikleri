-- Migration: Katlanabilir telefon, ekran 2, görsel galerisi, ters kablolu şarj vb. alanları
-- Bu SQL'i tek tek veya toplu çalıştırabilirsiniz.

-- 1. Katlanabilir ve boyut alanları
ALTER TABLE smartphones ADD COLUMN is_foldable TINYINT(1) NULL;
ALTER TABLE smartphones ADD COLUMN height_folded DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN width_folded DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN thickness_folded DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN height_unfolded DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN width_unfolded DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN thickness_unfolded DECIMAL(10,2) NULL;

-- 2. Ana ekran ek alanları
ALTER TABLE smartphones ADD COLUMN screen_touch_sampling_rate INT NULL;
ALTER TABLE smartphones ADD COLUMN screen_other_specs TEXT NULL;

-- 4. Kapak ekranı (Screen 2)
ALTER TABLE smartphones ADD COLUMN screen2_exists TINYINT(1) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_size DECIMAL(10,2) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_tech VARCHAR(100) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_res VARCHAR(100) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_refresh_rate INT NULL;
ALTER TABLE smartphones ADD COLUMN screen2_pixel_density INT NULL;
ALTER TABLE smartphones ADD COLUMN screen2_aspect_ratio VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_protection VARCHAR(100) NULL;
ALTER TABLE smartphones ADD COLUMN screen2_other_specs TEXT NULL;
ALTER TABLE smartphones ADD COLUMN screen2_touch_sampling_rate INT NULL;

-- 5. Ters kablolu şarj
ALTER TABLE smartphones ADD COLUMN reverse_wired TINYINT(1) NULL;
ALTER TABLE smartphones ADD COLUMN reverse_wired_speed INT NULL;

-- 6. Görsel galerisi (JSON - URL listesi)
ALTER TABLE smartphones ADD COLUMN image_gallery JSON NULL;

-- 7. Kapak ekranı özçekim kamerası
ALTER TABLE smartphones ADD COLUMN front_cover_exists TINYINT(1) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_res VARCHAR(50) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_aperture VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_focal VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_sensor_size VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_pixel_size VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_features TEXT NULL;
ALTER TABLE smartphones ADD COLUMN front_cover_video_res VARCHAR(255) NULL;
