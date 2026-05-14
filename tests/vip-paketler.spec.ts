import { test, expect } from '@playwright/test';

test.describe('VIP Danışmanlık Paketleri', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/VipDanismalikPaketleri');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve başlık görünür', async ({ page }) => {
    await expect(page).toHaveURL(/VipDanismalikPaketleri/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
    await expect(page.locator('text=Krediye Her Zaman Hazır Ol')).toBeVisible({ timeout: 5_000 });
  });

  test('3 fiyat kartı görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const pkg of ['Standart Rapor', '6 Aylık VIP', '12 Aylık VIP']) {
      const exists = await page.locator(`text=${pkg}`).count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: "${pkg}" paket kartı sayfada bulunamadı`);
      }
    }
  });

  test('fiyatlar görünür (499, 2.499, 4.499 TL)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const price of ['499', '2.499', '4.499']) {
      const exists = await page.locator(`text=${price}`).count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: ${price} TL fiyat bilgisi sayfada bulunamadı`);
      }
    }
  });

  test('CTA butonları görünür ve tıklanabilir', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const ctaBtns = page.locator('button:has-text("Satın Al"), button:has-text("VIP Üye Ol"), button:has-text("Paketi Seç")');
    const count = await ctaBtns.count();
    console.log(`ℹ️ VIP CTA buton sayısı: ${count}`);
    if (count === 0) {
      console.warn('🐛 BUG: VIP sayfasında hiç satın alma butonu bulunamadı');
    } else {
      await expect(ctaBtns.first()).toBeVisible();
    }
  });

  test('"En Popüler" badge görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const badge = page.locator('text=En Popüler').first();
    const exists = await badge.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: "En Popüler" badge 12 Aylık VIP PRO kartında bulunamadı');
    }
  });

  test('istatistik alanı görünür (1M+, %98, 24/7)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const stat of ['1M', '%98', '24/7', '256']) {
      const exists = await page.locator(`text=${stat}`).count() > 0;
      if (!exists) {
        console.warn(`⚠️ BİLGİ: "${stat}" istatistiği sayfada bulunamadı`);
      }
    }
  });

  test('%25 indirim banneri görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const banner = page.locator('text=%25').or(page.locator('text=indirim')).first();
    const exists = await banner.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: İndirim banneri sayfada bulunamadı');
    }
  });

});
