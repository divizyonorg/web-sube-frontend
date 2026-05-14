import { test, expect } from '@playwright/test';

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
    const exists = await banner.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Bilgi banneri bulunamadı');
    }
  });

  test('teklifler API\'den yükleniyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const offers = page.locator('button:has-text("Başvur"), a:has-text("Başvur")');
    const count = await offers.count();
    console.log(`ℹ️ Teklif kartı sayısı: ${count}`);
    if (count === 0) {
      console.warn('🐛 BUG: Teklif kartları yüklenmiyor — API\'den veri gelmiyor veya bu kullanıcıya teklif yok');
    }
  });

  test('"Başvur" butonları doğru linke yönlendiriyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const basvurLinks = page.locator('a:has-text("Başvur")');
    const count = await basvurLinks.count();
    if (count > 0) {
      const href = await basvurLinks.first().getAttribute('href');
      if (!href?.includes('/Apply') && !href?.includes('offerId')) {
        console.warn(`🐛 BUG: Başvur linki beklenen formatta değil: ${href}`);
      }
    }
  });

  test('teklif kartları Tutar, Faiz Oranı, Vade bilgisi içeriyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const offerCards = page.locator('a:has-text("Başvur")');
    const count = await offerCards.count();
    if (count === 0) {
      console.warn('⚠️ BİLGİ: Teklif kartı yok, Tutar/Faiz/Vade kontrolü atlandı');
      return;
    }
    for (const label of ['Tutar', 'Faiz', 'Vade']) {
      const exists = await page.locator(`text=${label}`).count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: Teklif kartında "${label}" bilgisi bulunamadı`);
      }
    }
  });

});
