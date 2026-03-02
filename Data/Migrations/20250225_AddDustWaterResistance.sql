-- Migration: Toz ve su dayanıklılığı sütunlarını ekle
-- Bu sütunlar mevcut değilse çalıştırın

ALTER TABLE smartphones ADD COLUMN dust_resistance VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN dust_resistance_exists TINYINT(1) NULL;
ALTER TABLE smartphones ADD COLUMN water_resistance VARCHAR(20) NULL;
ALTER TABLE smartphones ADD COLUMN water_resistance_exists TINYINT(1) NULL;
