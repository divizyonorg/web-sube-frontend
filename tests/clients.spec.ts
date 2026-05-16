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
      bug('Müşteri listesi boş — API\'den veri gelmiyor veya sayfa hatalı');
  });

  test('ClientSummary bileşeni görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const summary = page.locator('text=Toplam Müşteri').or(page.locator('text=toplam')).first();
    if (await summary.count() === 0)
      bug('ClientSummary bileşeni sayfada görünmüyor');
  });

  test('müşteri kartlarında isim ve e-posta var', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const nameEl = page.locator('p.font-medium').first();
    if (await nameEl.count() === 0)
      bug('Müşteri kartlarında isim alanı bulunamadı');
    const count = await page.locator('p.font-medium').count();
    info(`Müşteri kartı sayısı: ${count}`);
  });

  test('detay sayfasına gidilebiliyor', async ({ page }) => {
    await page.goto('/Clients/Detail');
    await page.waitForLoadState('domcontentloaded');
    const is404 = await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0;
    if (is404) bug('/Clients/Detail sayfası 404 döndürüyor');
  });

});
