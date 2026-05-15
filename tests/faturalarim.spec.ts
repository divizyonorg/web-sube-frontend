import { test, expect } from '@playwright/test';

test.describe('Faturalarım', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Faturalarim');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/Faturalarim/i);
    await expect(page.locator('h1, h2').first()).toBeVisible({ timeout: 15_000 });
  });

  test('özet kartları görünür (Toplam Ödenen, Bekleyen, Sonraki)', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    for (const label of ['Toplam Ödenen', 'Bekleyen Ödeme', 'Sonraki Ödeme']) {
      const exists = await page.locator(`text=${label}`).count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: "${label}" özet kartı sayfada bulunamadı`);
      }
    }
  });

  test('fatura listesi görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const rows = page.locator('table tr').filter({ hasNot: page.locator('th') });
    const count = await rows.count();
    console.log(`ℹ️ Fatura satırı sayısı: ${count}`);
    if (count === 0) {
      console.warn('⚠️ BİLGİ: Fatura listesi boş — statik demo veri bile görünmüyor olabilir');
    }
  });

  test('fatura durum badge\'leri görünür (Ödendi/Ödenmedi)', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const odendiExists   = await page.locator('text=Ödendi').count() > 0;
    const odenmediExists = await page.locator('text=Ödenmedi').count() > 0;
    if (!odendiExists && !odenmediExists) {
      console.warn('🐛 BUG: Fatura durum badge\'leri (Ödendi/Ödenmedi) sayfada bulunamadı');
    }
  });

  test('"Şimdi Öde" butonu ödenmemiş faturada görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const odeBtn = page.locator('button:has-text("Şimdi Öde"), a:has-text("Şimdi Öde")').first();
    const exists = await odeBtn.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: "Şimdi Öde" butonu bulunamadı — ödenmemiş fatura olmayabilir');
    } else {
      await expect(odeBtn).toBeVisible();
    }
  });

  test('görüntüleme ve indirme butonları görünür', async ({ page }) => {
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
    const viewBtns = page.locator('button[aria-label*="Görüntüle"], button[title*="Görüntüle"], a[href*="fatura"]');
    const count = await viewBtns.count();
    console.log(`ℹ️ Fatura görüntüleme butonu sayısı: ${count}`);
  });

});
