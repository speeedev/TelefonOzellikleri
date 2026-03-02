-- Migration: Smartphones tablosunu lookup FK'lara geçir
-- ÖNCE 20250225_CreateLookupTables.sql çalıştırılmalı
-- Mevcut verileri migrate eder, sonra eski sütunları kaldırır

-- ============================================
-- 1. YENİ SÜTUNLARI EKLE
-- ============================================

ALTER TABLE smartphones ADD COLUMN status_id INT NULL AFTER status;
ALTER TABLE smartphones ADD COLUMN os_type_id INT NULL AFTER os_type;
ALTER TABLE smartphones ADD COLUMN wifi_version_id INT NULL AFTER wifi_version;
ALTER TABLE smartphones ADD COLUMN battery_type_id INT NULL AFTER battery_type;
ALTER TABLE smartphones ADD COLUMN speaker_type_id INT NULL AFTER speaker_type;
ALTER TABLE smartphones ADD COLUMN usb_type_id INT NULL AFTER usb_type;
ALTER TABLE smartphones ADD COLUMN usb_version_id INT NULL AFTER usb_version;
ALTER TABLE smartphones ADD COLUMN storage_type_id INT NULL AFTER storage_type;
ALTER TABLE smartphones ADD COLUMN bluetooth_version_id INT NULL AFTER bluetooth_ver;
ALTER TABLE smartphones ADD COLUMN chipset_id INT NULL AFTER chipset;

-- ============================================
-- 2. MEVCUT VERİLERİ MİGRATE ET
-- Enum ordinal eşlemesi: 0=ilk, 1=ikinci, 2=üçüncü...
-- ============================================

-- status: 0=Released, 1=Unannounced, 2=Rumored
UPDATE smartphones SET status_id = 1 WHERE status = 0 OR status = '0';
UPDATE smartphones SET status_id = 2 WHERE status = 1 OR status = '1';
UPDATE smartphones SET status_id = 3 WHERE status = 2 OR status = '2';

-- os_type: 0=Android, 1=iOS, 2=HarmonyOS
UPDATE smartphones SET os_type_id = 1 WHERE os_type = 0 OR os_type = '0';
UPDATE smartphones SET os_type_id = 2 WHERE os_type = 1 OR os_type = '1';
UPDATE smartphones SET os_type_id = 3 WHERE os_type = 2 OR os_type = '2';

-- wifi_version: 0=WiFi4, 1=WiFi5, 2=WiFi6, 3=WiFi6E, 4=WiFi7
UPDATE smartphones SET wifi_version_id = 1 WHERE wifi_version = 0 OR wifi_version = '0';
UPDATE smartphones SET wifi_version_id = 2 WHERE wifi_version = 1 OR wifi_version = '1';
UPDATE smartphones SET wifi_version_id = 3 WHERE wifi_version = 2 OR wifi_version = '2';
UPDATE smartphones SET wifi_version_id = 4 WHERE wifi_version = 3 OR wifi_version = '3';
UPDATE smartphones SET wifi_version_id = 5 WHERE wifi_version = 4 OR wifi_version = '4';

-- battery_type: 0=LiIon, 1=LiPo
UPDATE smartphones SET battery_type_id = 1 WHERE battery_type = 0 OR battery_type = '0';
UPDATE smartphones SET battery_type_id = 2 WHERE battery_type = 1 OR battery_type = '1';

-- speaker_type: 0=Mono, 1=Stereo, 2=Surround
UPDATE smartphones SET speaker_type_id = 1 WHERE speaker_type = 0 OR speaker_type = '0';
UPDATE smartphones SET speaker_type_id = 2 WHERE speaker_type = 1 OR speaker_type = '1';
UPDATE smartphones SET speaker_type_id = 3 WHERE speaker_type = 2 OR speaker_type = '2';

-- String alanlar: slug veya name ile eşle (case insensitive)
UPDATE smartphones s
JOIN usb_types u ON LOWER(REPLACE(s.usb_type, ' ', '-')) = u.slug OR LOWER(s.usb_type) = LOWER(u.name)
SET s.usb_type_id = u.id
WHERE s.usb_type IS NOT NULL AND s.usb_type != '';

UPDATE smartphones s
JOIN usb_versions u ON LOWER(REPLACE(REPLACE(s.usb_version, ' ', '-'), '.', '-')) = u.slug OR s.usb_version = u.name
SET s.usb_version_id = u.id
WHERE s.usb_version IS NOT NULL AND s.usb_version != '';

UPDATE smartphones s
JOIN storage_types st ON LOWER(REPLACE(REPLACE(s.storage_type, ' ', '-'), '.', '-')) = st.slug OR LOWER(s.storage_type) = LOWER(st.name)
SET s.storage_type_id = st.id
WHERE s.storage_type IS NOT NULL AND s.storage_type != '';

UPDATE smartphones s
JOIN bluetooth_versions b ON LOWER(REPLACE(s.bluetooth_ver, '.', '-')) = b.slug OR s.bluetooth_ver LIKE CONCAT(b.name, '%')
SET s.bluetooth_version_id = b.id
WHERE s.bluetooth_ver IS NOT NULL AND s.bluetooth_ver != '';

UPDATE smartphones s
JOIN chipsets c ON LOWER(TRIM(s.chipset)) = LOWER(c.name)
SET s.chipset_id = c.id
WHERE s.chipset IS NOT NULL AND s.chipset != '';

-- ============================================
-- 3. FOREIGN KEY EKLE
-- ============================================

ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_status FOREIGN KEY (status_id) REFERENCES phone_statuses(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_os_type FOREIGN KEY (os_type_id) REFERENCES os_types(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_wifi_version FOREIGN KEY (wifi_version_id) REFERENCES wifi_versions(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_battery_type FOREIGN KEY (battery_type_id) REFERENCES battery_types(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_speaker_type FOREIGN KEY (speaker_type_id) REFERENCES speaker_types(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_usb_type FOREIGN KEY (usb_type_id) REFERENCES usb_types(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_usb_version FOREIGN KEY (usb_version_id) REFERENCES usb_versions(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_storage_type FOREIGN KEY (storage_type_id) REFERENCES storage_types(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_bluetooth_version FOREIGN KEY (bluetooth_version_id) REFERENCES bluetooth_versions(id) ON DELETE SET NULL;
ALTER TABLE smartphones ADD CONSTRAINT fk_smartphones_chipset FOREIGN KEY (chipset_id) REFERENCES chipsets(id) ON DELETE SET NULL;

-- ============================================
-- 4. ESKİ SÜTUNLARI KALDIR
-- ============================================

ALTER TABLE smartphones DROP COLUMN status;
ALTER TABLE smartphones DROP COLUMN os_type;
ALTER TABLE smartphones DROP COLUMN wifi_version;
ALTER TABLE smartphones DROP COLUMN battery_type;
ALTER TABLE smartphones DROP COLUMN speaker_type;
ALTER TABLE smartphones DROP COLUMN usb_type;
ALTER TABLE smartphones DROP COLUMN usb_version;
ALTER TABLE smartphones DROP COLUMN storage_type;
ALTER TABLE smartphones DROP COLUMN bluetooth_ver;
ALTER TABLE smartphones DROP COLUMN chipset;
