-- Migration: reverse_speed sütununu kaldır
-- Ters kablosuz şarj hızı artık tutulmuyor

ALTER TABLE smartphones DROP COLUMN reverse_speed;
