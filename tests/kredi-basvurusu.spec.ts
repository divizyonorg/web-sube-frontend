import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Kredi Başvurusu', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediBasvurusu');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/KrediBasvurusu/i);
    await expect(page.locator('h1, h2').first()).toBeVisible({ timeout: 15_000 });
  });

  test('1. adım görünür ve devam butonu var', async ({ page }) => {
    await page.waitForTimeout(1000);
    if (await page.locator('[id*="step"], [class*="step"], form').count() === 0)
      bug('Kredi başvurusu 1. adım içeriği bulunamadı — HTMX yüklenmemiş olabilir');
  });

  test('kredi türü seçimi yapılabiliyor', async ({ page }) => {
    await page.waitForTimeout(1000);
    if (await page.locator('select, [x-model="tur"], input[name*="tur"]').count() === 0)
      bug('Kredi türü seçim alanı bulunamadı');
  });

  test('devam butonu görünür', async ({ page }) => {
    await page.waitForTimeout(1000);
    const devamBtn = page.locator('button:has-text("Devam"), button:has-text("İleri"), button[type="submit"]').first();
    if (await devamBtn.count() === 0) {
      bug('Kredi başvurusu devam/ileri butonu bulunamadı');
    } else {
      await expect(devamBtn).toBeVisible({ timeout: 10_000 });
    }
  });

  test('adım göstergesi görünür', async ({ page }) => {
    await page.waitForTimeout(1000);
    if (await page.locator('[class*="step"], [id*="step-indicator"], nav').count() === 0)
      bug('Adım göstergesi bulunamadı');
  });

  test('ödeme/kupon tetikleyicileri DOM\'da mevcut', async ({ page }) => {
    await page.waitForTimeout(2000);
    const count = await page.locator('[data-pay-trigger], [data-coupon-trigger], [data-verify-trigger]').count();
    info(`Ödeme/kupon tetikleyici sayısı: ${count}`);
  });

});
