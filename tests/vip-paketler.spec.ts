import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('VIP Danışmanlık Paketleri', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/VipDanismalikPaketleri');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve başlık görünür', async ({ page }) => {
    await expect(page).toHaveURL(/VipDanismalikPaketleri/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
    if (await page.locator('text=Krediye Her Zaman Hazır Ol').count() === 0)
      bug('Sayfa başlığı "Krediye Her Zaman Hazır Ol" görünmüyor');
  });

  test('3 fiyat kartı görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const pkg of ['Standart Rapor', '6 Aylık VIP', '12 Aylık VIP']) {
      if (await page.locator(`text=${pkg}`).count() === 0)
        bug(`"${pkg}" paket kartı sayfada bulunamadı`);
    }
  });

  test('fiyatlar görünür (499, 2.499, 4.499 TL)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const price of ['499', '2.499', '4.499']) {
      if (await page.locator(`text=${price}`).count() === 0)
        bug(`${price} TL fiyat bilgisi sayfada bulunamadı`);
    }
  });

  test('CTA butonları görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const ctaBtns = page.locator('button:has-text("Satın Al"), button:has-text("VIP Üye Ol"), button:has-text("Paketi Seç")');
    const count = await ctaBtns.count();
    info(`VIP CTA buton sayısı: ${count}`);
    if (count === 0)
      bug('VIP sayfasında hiç satın alma butonu bulunamadı');
    else
      await expect(ctaBtns.first()).toBeVisible();
  });

  test('"En Popüler" badge görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    if (await page.locator('text=En Popüler').count() === 0)
      bug('"En Popüler" badge 12 Aylık VIP PRO kartında bulunamadı');
  });

  test('istatistik alanı görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const stat of ['1M', '%98', '24/7', '256']) {
      if (await page.locator(`text=${stat}`).count() === 0)
        bug(`"${stat}" istatistiği sayfada bulunamadı`);
    }
  });

  test('indirim banneri görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const banner = page.locator('text=%25').or(page.locator('text=indirim')).first();
    if (await banner.count() === 0)
      bug('İndirim banneri sayfada bulunamadı');
  });

});
