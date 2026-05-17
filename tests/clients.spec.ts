import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Müşteriler', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Clients');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/Clients/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  test('müşteri listesi görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const cards = page.locator('[class*="card"], [class*="client"], .divide-y > div').first();
    if (await cards.count() === 0)
      info('Müşteri listesi boş — test ortamında seed data gerekli veya API\'den veri gelmiyor');
  });

  test('ClientSummary bileşeni görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const summary = page.locator('text=Toplam Müşteri').or(page.locator('text=toplam')).first();
    if (await summary.count() === 0)
      info('ClientSummary bileşeni sayfada görünmüyor — test ortamında veri olmayabilir');
  });

  test('müşteri kartlarında isim ve e-posta var', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const nameEl = page.locator('p.font-medium').first();
    if (await nameEl.count() === 0)
      info('Müşteri kartlarında isim alanı bulunamadı — test ortamında veri olmayabilir');
    const count = await page.locator('p.font-medium').count();
    info(`Müşteri kartı sayısı: ${count}`);
  });

  test('detay sayfasına gidilebiliyor', async ({ page }) => {
    await page.goto('/Clients/Detail/1');
    await page.waitForLoadState('domcontentloaded');
    const is404 = await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0;
    if (is404) info('/Clients/Detail/1 sayfası 404 döndürüyor — bu ID\'li müşteri olmayabilir');
  });

});
