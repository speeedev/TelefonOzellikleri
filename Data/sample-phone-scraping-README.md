# Örnek Telefon JSON (Scraping Şablonu)

- **sample-phone-scraping.json** – Yorum içermeyen, import için kullanılacak temiz JSON
- **sample-phone-scraping.jsonc** – Her alanın yanında açıklama yorumu olan referans dosyası (JSONC formatı)

## Enum Değerleri

- **status:** `Released`, `Unannounced`, `Rumored`
- **osType:** `Android`, `iOS`, `HarmonyOS`
- **battery.type:** `LiIon`, `LiPo`
- **connectivity.wifiVersion:** `WiFi4`, `WiFi5`, `WiFi6`, `WiFi6E`, `WiFi7`
- **connectivity.speakerType:** `Mono`, `Stereo`, `Surround`

## DTO'da Olmayan Alanlar

`SmartphoneJsonDto` şu anda şu alanları desteklemiyor; scraping için eklenebilir:

- **general.imageGallery** – Galeri resim URL'leri listesi
- **display.touchSamplingRate** – Dokunmatik örnekleme hızı (Hz)
- **display.screen2** – Katlanabilir telefonlar için ikinci ekran (size, tech, resolution, vb.)
- **design.isFoldable** – Katlanabilir mi?
- **design.foldedDimensions** – Katlanmış boyutlar (height, width, thickness)
- **design.unfoldedDimensions** – Açılmış boyutlar
- **connectivity.headphoneJack35mm** – 3.5 mm kulaklık girişi

## Notlar

- `brandId` ve `seriesId` veritabanında mevcut olmalıdır; yoksa `brand` ve `series` string isimleri ile eşleme yapılabilir.
- `frontCover.exists: false` ise `frontCover` objesi tamamen atlanabilir veya null bırakılabilir.
- `camera.rear` için `main`, `secondary`, `tertiary`, `quaternary` – kamera yoksa ilgili obje null veya atlanabilir.
