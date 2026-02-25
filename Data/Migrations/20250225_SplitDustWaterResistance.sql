-- Migration: Split dust_water_res into dust_resistance and water_resistance
-- Run this SQL against your database before deploying the code changes.

-- 1. Add new columns
ALTER TABLE smartphones ADD COLUMN dust_resistance VARCHAR(20) NULL AFTER screen_protection;
ALTER TABLE smartphones ADD COLUMN water_resistance VARCHAR(20) NULL AFTER dust_resistance;

-- 2. Migrate data from dust_water_res (e.g. IP68 -> dust=6, water=8)
UPDATE smartphones
SET
    dust_resistance = CASE
        WHEN dust_water_res LIKE 'IP%' AND LENGTH(dust_water_res) >= 4
             AND SUBSTRING(dust_water_res, 3, 1) BETWEEN '0' AND '9'
        THEN SUBSTRING(dust_water_res, 3, 1)
        ELSE NULL
    END,
    water_resistance = CASE
        WHEN dust_water_res LIKE 'IP%' AND LENGTH(dust_water_res) >= 4
             AND SUBSTRING(dust_water_res, 4, 1) BETWEEN '0' AND '9'
        THEN SUBSTRING(dust_water_res, 4, 1)
        ELSE NULL
    END
WHERE dust_water_res IS NOT NULL AND dust_water_res != '';

-- 3. For unparseable values (e.g. "IPX8", "IP6X"), put in appropriate column
UPDATE smartphones
SET water_resistance = SUBSTRING(dust_water_res, 4, 1)
WHERE dust_water_res IS NOT NULL
  AND dust_water_res REGEXP '^IPX[0-9]$'
  AND water_resistance IS NULL;

UPDATE smartphones
SET dust_resistance = SUBSTRING(dust_water_res, 3, 1)
WHERE dust_water_res IS NOT NULL
  AND dust_water_res REGEXP '^IP[0-9]X$'
  AND dust_resistance IS NULL;

-- 4. Fallback: store whole value in dust_resistance if neither parsed
UPDATE smartphones
SET dust_resistance = dust_water_res
WHERE dust_water_res IS NOT NULL
  AND dust_resistance IS NULL
  AND water_resistance IS NULL;

-- 5. Drop old column
ALTER TABLE smartphones DROP COLUMN dust_water_res;
