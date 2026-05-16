import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

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
    for (const [name, locator] of [
      ['Toplam Rapor', page.locator('text=Toplam Rapor').first()],
      ['Hazır Rapor',  page.locator('text=Hazır Rapor').first()],
      ['İşleniyor',    page.locator('text=İşleniyor').first()],
    ] as const) {
      if (await locator.count() === 0)
        bug(`"${name}" özet kartı sayfada bulunamadı`);
    }
  });

  test('rapor listesi yükleniyor', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const count = await page.locator('table tr, [class*="rapor"], [class*="report"]').count();
    info(`Rapor satırı sayısı: ${count}`);
    if (count === 0)
      bug('Rapor listesi boş — bu kullanıcıya ait rapor olmayabilir');
  });

  test('"Yeni Rapor Oluştur" butonu görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const yeniBtn = page.locator('button:has-text("Yeni Rapor"), a:has-text("Yeni Rapor")').first();
    if (await yeniBtn.count() === 0) {
      bug('"Yeni Rapor Oluştur" butonu sayfada bulunamadı');
    } else {
      await expect(yeniBtn).toBeVisible({ timeout: 5_000 });
    }
  });

  test('rapor varsa görüntüleme butonu görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const viewBtn = page.locator('button:has-text("Raporu Gör"), a:has-text("Raporu Gör")').first();
    if (await viewBtn.count() > 0)
      await expect(viewBtn).toBeVisible();
  });

  test('KurDetay sayfasına gidiliyor', async ({ page }) => {
    await page.goto('/KrediRaporlari/KurDetay');
    await page.waitForLoadState('domcontentloaded');
    if (await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0)
      bug('/KrediRaporlari/KurDetay sayfası 404 döndürüyor');
  });

  // ─── Bug 4: "Raporu Gör" yanlış yönlendiriyor ────────────────────────────
  test('[BUG-4] Farklı raporlar için "Raporu Gör" farklı URL\'e gitmeli', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const viewBtns = page.locator('button:has-text("Raporu Gör"), a:has-text("Raporu Gör")');
    const count = await viewBtns.count();
    info(`"Raporu Gör" buton sayısı: ${count}`);
    if (count < 2) {
      info('[BUG-4] Test için en az 2 rapor gerekli — bu kullanıcıda yeterli rapor olmayabilir');
      return;
    }

    // 1. rapor
    await viewBtns.nth(0).click({ force: true });
    await page.waitForLoadState('domcontentloaded', { timeout: 10_000 });
    const firstUrl = page.url();
    info(`1. rapor URL: ${firstUrl}`);

    await page.goto('/KrediRaporlari');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });

    // 2. rapor
    const viewBtns2 = page.locator('button:has-text("Raporu Gör"), a:has-text("Raporu Gör")');
    await viewBtns2.nth(1).click({ force: true });
    await page.waitForLoadState('domcontentloaded', { timeout: 10_000 });
    const secondUrl = page.url();
    info(`2. rapor URL: ${secondUrl}`);

    if (firstUrl === secondUrl)
      bug(`[BUG-4] Farklı raporlar için "Raporu Gör" aynı URL'e yönlendiriyor: ${firstUrl} — yönlendirme sabit`);
  });

});
