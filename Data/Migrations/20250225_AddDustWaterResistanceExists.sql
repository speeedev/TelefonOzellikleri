-- Migration: Add dust_resistance_exists and water_resistance_exists boolean columns
-- Run after 20250225_SplitDustWaterResistance.sql (or when dust_resistance/water_resistance columns exist)

ALTER TABLE smartphones ADD COLUMN dust_resistance_exists TINYINT(1) NULL AFTER dust_resistance;
ALTER TABLE smartphones ADD COLUMN water_resistance_exists TINYINT(1) NULL AFTER water_resistance;
