-- Migration: Ters kablosuz şarj hızı sütunu ekle
ALTER TABLE smartphones ADD COLUMN reverse_wireless_speed INT NULL;
