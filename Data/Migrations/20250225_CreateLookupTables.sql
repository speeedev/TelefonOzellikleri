-- Migration: Lookup tabloları - Panelden yönetilebilir enum değerleri
-- Çalıştırma sırası: 1) Bu dosya  2) 20250225_AlterSmartphonesForLookups.sql (veri migrate edildikten sonra)

-- ============================================
-- 1. LOOKUP TABLOLARI
-- ============================================

CREATE TABLE phone_statuses (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE os_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE wifi_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE battery_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE speaker_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE usb_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE usb_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE storage_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE bluetooth_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE chipsets (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    slug VARCHAR(150) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================
-- 2. VARSAYILAN VERİLER (mevcut enum değerleri)
-- ============================================

INSERT INTO phone_statuses (name, slug, sort_order) VALUES
('Released', 'released', 1),
('Unannounced', 'unannounced', 2),
('Rumored', 'rumored', 3);

INSERT INTO os_types (name, slug, sort_order) VALUES
('Android', 'android', 1),
('iOS', 'ios', 2),
('HarmonyOS', 'harmonyos', 3);

INSERT INTO wifi_versions (name, slug, sort_order) VALUES
('Wi-Fi 4', 'wifi4', 1),
('Wi-Fi 5', 'wifi5', 2),
('Wi-Fi 6', 'wifi6', 3),
('Wi-Fi 6E', 'wifi6e', 4),
('Wi-Fi 7', 'wifi7', 5);

INSERT INTO battery_types (name, slug, sort_order) VALUES
('Li-Ion', 'liion', 1),
('Li-Po', 'lipo', 2);

INSERT INTO speaker_types (name, slug, sort_order) VALUES
('Mono', 'mono', 1),
('Stereo', 'stereo', 2),
('Surround', 'surround', 3);

INSERT INTO usb_types (name, slug, sort_order) VALUES
('USB Type-C', 'usb-type-c', 1),
('Micro USB', 'micro-usb', 2),
('Lightning', 'lightning', 3);

INSERT INTO usb_versions (name, slug, sort_order) VALUES
('2.0', '2-0', 1),
('3.0', '3-0', 2),
('3.1', '3-1', 3),
('3.2', '3-2', 4),
('DisplayPort', 'displayport', 5);

INSERT INTO storage_types (name, slug, sort_order) VALUES
('NVMe', 'nvme', 1),
('UFS 2.0', 'ufs-2-0', 2),
('UFS 2.1', 'ufs-2-1', 3),
('UFS 3.0', 'ufs-3-0', 4),
('UFS 3.1', 'ufs-3-1', 5),
('UFS 4.0', 'ufs-4-0', 6),
('eMMC', 'emmc', 7);

INSERT INTO bluetooth_versions (name, slug, sort_order) VALUES
('4.2', '4-2', 1),
('5.0', '5-0', 2),
('5.1', '5-1', 3),
('5.2', '5-2', 4),
('5.3', '5-3', 5),
('5.4', '5-4', 6);

INSERT INTO chipsets (name, slug, sort_order) VALUES
('Apple A16 Bionic (4 nm)', 'apple-a16-bionic-4-nm', 1),
('Apple A17 Pro (3 nm)', 'apple-a17-pro-3-nm', 2),
('Snapdragon 8 Gen 2', 'snapdragon-8-gen-2', 3),
('Snapdragon 8 Gen 3', 'snapdragon-8-gen-3', 4),
('Dimensity 9200', 'dimensity-9200', 5),
('Dimensity 9300', 'dimensity-9300', 6),
('Exynos 2400', 'exynos-2400', 7),
('Tensor G3', 'tensor-g3', 8);
