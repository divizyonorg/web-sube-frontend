import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Sana Özel Teklifler', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/SanaOzelTeklifler');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/SanaOzelTeklifler/i);
    await expect(page.locator('h1, h2').first()).toBeVisible({ timeout: 15_000 });
  });

  test('bilgi banneri görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const banner = page.locator('[class*="banner"], [class*="info"], [class*="uyari"]').first();
    if (await banner.count() === 0) bug('Bilgi banneri bulunamadı');
  });

  test('teklifler API\'den yükleniyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const offers = page.locator('button:has-text("Başvur"), a:has-text("Başvur")');
    const count = await offers.count();
    info(`Teklif kartı sayısı: ${count}`);
    if (count === 0) bug('Teklif kartları yüklenmiyor — API\'den veri gelmiyor veya bu kullanıcıya teklif yok');
  });

  test('"Başvur" butonları doğru linke yönlendiriyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const basvurLinks = page.locator('a:has-text("Başvur")');
    if (await basvurLinks.count() > 0) {
      const href = await basvurLinks.first().getAttribute('href');
      if (!href) bug('Başvur linkinin href değeri boş');
    }
  });

  test('teklif kartları Tutar, Faiz Oranı, Vade bilgisi içeriyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const offerCards = page.locator('a:has-text("Başvur")');
    if (await offerCards.count() === 0) return;
    for (const label of ['Tutar', 'Faiz', 'Vade']) {
      if (await page.locator(`text=${label}`).count() === 0)
        bug(`Teklif kartında "${label}" bilgisi bulunamadı`);
    }
  });

});
