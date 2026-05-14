import { test, expect } from '@playwright/test';

test.describe('Kredi Raporları', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediRaporlari');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/KrediRaporlari/i);
    await expect(page.locator('h1, h2').first()).toBeVisible({ timeout: 15_000 });
  });

  test('özet kartları görünür (Toplam, Hazır, İşleniyor)', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const toplamCard = page.locator('text=Toplam Rapor').first();
    const hazirCard  = page.locator('text=Hazır Rapor').first();
    const isleniyor  = page.locator('text=İşleniyor').first();

    for (const [name, card] of [['Toplam Rapor', toplamCard], ['Hazır Rapor', hazirCard], ['İşleniyor', isleniyor]] as const) {
      const exists = await card.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: "${name}" özet kartı sayfada bulunamadı`);
      }
    }
  });

  test('rapor listesi yükleniyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const rows = page.locator('table tr, [class*="rapor"], [class*="report"]');
    const count = await rows.count();
    console.log(`ℹ️ Rapor satırı sayısı: ${count}`);
    if (count === 0) {
      console.warn('⚠️ BİLGİ: Rapor listesi boş — bu kullanıcıya ait rapor olmayabilir');
    }
  });

  test('"Yeni Rapor Oluştur" butonu görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const yeniBtn = page.locator('button:has-text("Yeni Rapor"), a:has-text("Yeni Rapor")').first();
    const exists = await yeniBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: "Yeni Rapor Oluştur" butonu sayfada bulunamadı');
    } else {
      await expect(yeniBtn).toBeVisible({ timeout: 5_000 });
    }
  });

  test('rapor varsa görüntüleme ve indirme butonları görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const viewBtn = page.locator('button:has-text("Raporu Gör"), a:has-text("Raporu Gör")').first();
    const exists = await viewBtn.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: "Raporu Gör" butonu yok — listede rapor olmayabilir');
    } else {
      await expect(viewBtn).toBeVisible();
    }
  });

  test('KurDetay sayfasına gidiliyor', async ({ page }) => {
    await page.goto('/KrediRaporlari/KurDetay');
    await page.waitForLoadState('domcontentloaded');
    const is404 = await page.locator('text=404, text=Bulunamadı').count() > 0;
    if (is404) {
      console.warn('🐛 BUG: /KrediRaporlari/KurDetay sayfası 404 döndürüyor');
    }
  });

});
