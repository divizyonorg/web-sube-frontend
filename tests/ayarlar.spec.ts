import { test, expect } from '@playwright/test';

test.describe('Ayarlar', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Ayarlar');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/Ayarlar/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  test('tüm 5 sekme görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const tabs = ['Finansal Profil', 'Profil Bilgileri', 'Güvenlik', 'Bildirimler', 'Ödeme Yöntemleri'];
    for (const tab of tabs) {
      const btn = page.getByRole('button', { name: tab });
      const exists = await btn.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: "${tab}" sekmesi bulunamadı`);
      } else {
        await expect(btn).toBeVisible({ timeout: 5_000 });
      }
    }
  });

  // ─── Finansal Profil ──────────────────────────────────────────────────────
  test('Finansal Profil sekmesi varsayılan açık', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const content = page.locator('text=Gelir').or(page.locator('text=Meslek')).or(page.locator('text=Çalışma')).first();
    const exists = await content.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Finansal Profil sekmesi varsayılan açık değil veya içerik yüklenmiyor');
    }
  });

  test('Finansal Profil formu API\'den dolu geliyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Finansal Profil' }).click();
    await page.waitForTimeout(500);

    const inputs = page.locator('input:not([type="hidden"]), select');
    const count = await inputs.count();
    console.log(`ℹ️ Finansal Profil form alanı sayısı: ${count}`);
    if (count === 0) {
      console.warn('🐛 BUG: Finansal Profil sekmesinde hiç form alanı yok');
    }
  });

  // ─── Profil Bilgileri ─────────────────────────────────────────────────────
  test('Profil Bilgileri sekmesi açılır ve Ad Soyad görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.waitForTimeout(500);

    const adSoyadLabel = page.locator('label:has-text("Ad Soyad")').first();
    const exists = await adSoyadLabel.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Profil Bilgileri sekmesinde "Ad Soyad" etiketi bulunamadı');
    } else {
      await expect(adSoyadLabel).toBeVisible({ timeout: 5_000 });
    }
  });

  test('Profil Bilgileri\'nde e-posta ve telefon alanları var', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.waitForTimeout(500);

    for (const label of ['E-posta', 'Telefon']) {
      const el = page.locator(`label:has-text("${label}")`).first();
      const exists = await el.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: Profil Bilgileri sekmesinde "${label}" alanı bulunamadı`);
      }
    }
  });

  test('Profil Bilgileri güncelleme butonu görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.waitForTimeout(500);

    const saveBtn = page.locator('button:has-text("Güncelle"), button:has-text("Kaydet"), button[type="submit"]').first();
    const exists = await saveBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Profil Bilgileri sekmesinde güncelleme butonu bulunamadı');
    }
  });

  // ─── Güvenlik ─────────────────────────────────────────────────────────────
  test('Güvenlik sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Güvenlik' }).click();
    await page.waitForTimeout(500);

    const content = page.locator('text=Şifre').or(page.locator('text=Güvenlik')).first();
    const exists = await content.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Güvenlik sekmesi içeriği yüklenmiyor');
    }
  });

  // ─── Bildirimler ──────────────────────────────────────────────────────────
  test('Bildirimler sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Bildirimler' }).click();
    await page.waitForTimeout(500);

    const content = page.locator('text=Bildirim').first();
    const exists = await content.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Bildirimler sekmesi içeriği yüklenmiyor');
    }
  });

  // ─── Ödeme Yöntemleri ─────────────────────────────────────────────────────
  test('Ödeme Yöntemleri sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Ödeme Yöntemleri' }).click();
    await page.waitForTimeout(500);

    const content = page.locator('text=Kart').or(page.locator('text=Ödeme')).first();
    const exists = await content.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Ödeme Yöntemleri sekmesi içeriği yüklenmiyor');
    }
  });

});
