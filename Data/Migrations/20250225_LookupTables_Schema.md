# Lookup Tabloları - Veritabanı Planı

Panelden yönetilebilir enum benzeri değerler için lookup tabloları tasarımı.

---

## 1. Ortak Tablo Yapısı

Tüm lookup tabloları aynı yapıyı kullanır:

| Sütun | Tip | Açıklama |
|-------|-----|----------|
| id | INT, PK, AUTO_INCREMENT | Birincil anahtar |
| name | VARCHAR(100), NOT NULL | Görüntüleme adı (örn: "iOS", "WiFi 6") |
| slug | VARCHAR(100), UNIQUE, NOT NULL | URL/API için (örn: "ios", "wifi-6") |
| sort_order | INT, DEFAULT 0 | Sıralama (küçük = önce) |
| is_active | TINYINT(1), DEFAULT 1 | 0 = pasif (silinmiş sayılır, mevcut kayıtlarda kullanılabilir) |

---

## 2. Lookup Tabloları

### phone_statuses (Telefon Durumu)
```sql
CREATE TABLE phone_statuses (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Released, Unannounced, Rumored, Discontinued
```

### os_types (İşletim Sistemi)
```sql
CREATE TABLE os_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Android, iOS, HarmonyOS
```

### wifi_versions (Wi-Fi Sürümü)
```sql
CREATE TABLE wifi_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Wi-Fi 4, Wi-Fi 5, Wi-Fi 6, Wi-Fi 6E, Wi-Fi 7
```

### battery_types (Batarya Tipi)
```sql
CREATE TABLE battery_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Li-Ion, Li-Po
```

### speaker_types (Hoparlör Tipi)
```sql
CREATE TABLE speaker_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Mono, Stereo, Surround
```

### usb_types (USB Tipi)
```sql
CREATE TABLE usb_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: USB Type-C, Micro USB, Lightning
```

### usb_versions (USB Sürümü)
```sql
CREATE TABLE usb_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: 2.0, 3.2, DisplayPort
```

### storage_types (Depolama Tipi)
```sql
CREATE TABLE storage_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: NVMe, UFS 4.0, eMMC
```

### bluetooth_versions (Bluetooth Sürümü)
```sql
CREATE TABLE bluetooth_versions (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: 5.0, 5.1, 5.2, 5.3, 5.4
```

### chipsets (İşlemci / Chipset)
```sql
CREATE TABLE chipsets (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    slug VARCHAR(150) NOT NULL UNIQUE,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1
);
-- Örnek: Apple A16 Bionic (4 nm), Snapdragon 8 Gen 2, Dimensity 9200
```

---

## 3. Smartphones Tablosu Değişiklikleri

Mevcut enum/string sütunlar **foreign key** ile değiştirilir:

| Eski Sütun | Yeni Sütun | Tip | FK |
|------------|------------|-----|-----|
| status | status_id | INT NULL | → phone_statuses(id) |
| os_type | os_type_id | INT NULL | → os_types(id) |
| wifi_version | wifi_version_id | INT NULL | → wifi_versions(id) |
| battery_type | battery_type_id | INT NULL | → battery_types(id) |
| speaker_type | speaker_type_id | INT NULL | → speaker_types(id) |
| usb_type | usb_type_id | INT NULL | → usb_types(id) |
| usb_version | usb_version_id | INT NULL | → usb_versions(id) |
| storage_type | storage_type_id | INT NULL | → storage_types(id) |
| bluetooth_ver | bluetooth_version_id | INT NULL | → bluetooth_versions(id) |
| chipset | chipset_id | INT NULL | → chipsets(id) |

**Not:** `bluetooth_ver` string idi (örn: "5.3, A2DP, LE"). Lookup'ta `name` alanına tam metin girilebilir. `chipset` string idi (örn: "Apple A16 Bionic (4 nm)").

---

## 4. Migration Adımları

### Adım 1: Lookup tablolarını oluştur
### Adım 2: Varsayılan verileri ekle (mevcut enum değerleri)
### Adım 3: Smartphones'a yeni _id sütunlarını ekle
### Adım 4: Mevcut verileri migrate et (ordinal/string → yeni id)
### Adım 5: Eski sütunları kaldır

---

## 5. Alternatif: Tekil Lookup Tablosu (Category + Value)

Tüm lookup'ları tek tabloda tutmak istersen:

```sql
CREATE TABLE lookup_values (
    id INT PRIMARY KEY AUTO_INCREMENT,
    category VARCHAR(50) NOT NULL,  -- 'status', 'os_type', 'wifi_version' vb.
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL,
    sort_order INT DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1,
    UNIQUE KEY uk_category_slug (category, slug)
);
```

**Artı:** Tek tablo, yeni kategori eklemek kolay  
**Eksi:** Her kategori için ayrı FK yok, JOIN'ler daha karmaşık, tip güvenliği zayıf

**Öneri:** Her lookup için ayrı tablo kullanmak daha temiz ve EF Core ile daha uyumlu.

---

## 6. Migration Dosyaları

| Dosya | Açıklama |
|-------|----------|
| `20250225_CreateLookupTables.sql` | Lookup tablolarını oluşturur + varsayılan verileri ekler |
| `20250225_AlterSmartphonesForLookups.sql` | Smartphones'a _id sütunları ekler, veriyi migrate eder, eski sütunları kaldırır |

**Önemli:** `AlterSmartphonesForLookups` çalıştırmadan önce, mevcut `usb_type`, `usb_version`, `storage_type`, `bluetooth_ver`, `chipset` değerlerinin lookup tablolarında karşılığı olduğundan emin olun. Eşleşmeyen değerler için önce ilgili lookup tablosuna INSERT yapın. Chipset eşleşmesi `name` alanına göre (büyük/küçük harf duyarsız) yapılır.

---

## 7. Admin Panel Yapısı (Öneri)

- `/derin/lookups` — Tüm lookup türlerini listele
- `/derin/lookups/status` — Telefon durumlarını yönet
- `/derin/lookups/os-types` — İşletim sistemlerini yönet
- `/derin/lookups/wifi-versions` — Wi-Fi sürümlerini yönet
- `/derin/lookups/battery-types` — Batarya tiplerini yönet
- `/derin/lookups/speaker-types` — Hoparlör tiplerini yönet
- `/derin/lookups/usb-types` — USB tiplerini yönet
- `/derin/lookups/usb-versions` — USB sürümlerini yönet
- `/derin/lookups/storage-types` — Depolama tiplerini yönet
- `/derin/lookups/bluetooth-versions` — Bluetooth sürümlerini yönet
- `/derin/lookups/chipsets` — Chipset'leri yönet

Her sayfada: Liste (tablo) + Ekle butonu + Düzenle/Sil aksiyonları.
