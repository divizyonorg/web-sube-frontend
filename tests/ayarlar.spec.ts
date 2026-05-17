import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

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
    for (const tab of ['Finansal Profil', 'Profil Bilgileri', 'Güvenlik', 'Bildirimler', 'Ödeme Yöntemleri']) {
      const btn = page.getByRole('button', { name: tab });
      if (await btn.count() === 0) {
        bug(`"${tab}" sekmesi bulunamadı`);
      } else {
        await expect(btn).toBeVisible({ timeout: 5_000 });
      }
    }
  });

  test('Finansal Profil sekmesi varsayılan açık', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const content = page.locator('text=Gelir').or(page.locator('text=Meslek')).or(page.locator('text=Çalışma')).first();
    if (await content.count() === 0)
      bug('Finansal Profil sekmesi varsayılan açık değil veya içerik yüklenmiyor');
  });

  test('Finansal Profil formu API\'den dolu geliyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Finansal Profil' }).click();
    await page.locator('input:not([type="hidden"]), select').first().waitFor({ state: 'visible', timeout: 5_000 }).catch(() => {});
    const count = await page.locator('input:not([type="hidden"]), select').count();
    info(`Finansal Profil form alanı sayısı: ${count}`);
    if (count === 0)
      bug('Finansal Profil sekmesinde hiç form alanı yok');
  });

  // ─── Bug 5: Profil Bilgileri kaydedilince bildirim yok ────────────────────
  test('[BUG-5] Profil Bilgileri kaydedilince başarı bildirimi gösterilmeli', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.locator('label:has-text("Ad Soyad"), input').first().waitFor({ state: 'visible', timeout: 5_000 }).catch(() => {});
    const saveBtn = page.locator('button:has-text("Kaydet"), button:has-text("Güncelle"), button[type="submit"]').first();
    if (await saveBtn.count() === 0) { bug('[BUG-5] Kaydet butonu bulunamadı'); return; }
    await saveBtn.click({ force: true });
    await page.waitForResponse(
      resp => resp.url().includes('/Ayarlar') && resp.status() === 200,
      { timeout: 10_000 }
    ).catch(() => {});
    const toast = page.locator('[data-toast]').first();
    if (await toast.count() === 0)
      bug('[BUG-5] Profil Bilgileri kaydedildiğinde başarı/bildirim mesajı gösterilmiyor');
  });

  // ─── Bug 6: Finansal/Demografik Profil kaydedilince bildirim yok ──────────
  test('[BUG-6] Finansal Profil kaydedilince başarı bildirimi gösterilmeli', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Finansal Profil' }).click();
    await page.locator('input:not([type="hidden"]), select').first().waitFor({ state: 'visible', timeout: 5_000 }).catch(() => {});
    const saveBtn = page.locator('button:has-text("Kaydet"), button[type="submit"]').first();
    if (await saveBtn.count() === 0) { bug('[BUG-6] Finansal Profil kaydet butonu bulunamadı'); return; }
    await saveBtn.click({ force: true });
    await page.waitForResponse(
      resp => resp.url().includes('/Ayarlar') && resp.status() === 200,
      { timeout: 10_000 }
    ).catch(() => {});
    const toast = page.locator('[data-toast]').first();
    if (await toast.count() === 0)
      bug('[BUG-6] Finansal/Demografik Profil kaydedildiğinde başarı bildirimi gösterilmiyor');
  });

  test('Profil Bilgileri sekmesi açılır ve Ad Soyad görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    const adSoyadLabel = page.locator('label:has-text("Ad Soyad")').first();
    if (await adSoyadLabel.count() === 0) {
      bug('Profil Bilgileri sekmesinde "Ad Soyad" etiketi bulunamadı');
    } else {
      await expect(adSoyadLabel).toBeVisible({ timeout: 5_000 });
    }
  });

  test('Profil Bilgileri\'nde e-posta ve telefon alanları var', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.locator('label').first().waitFor({ state: 'visible', timeout: 5_000 }).catch(() => {});
    for (const label of ['E-posta', 'Telefon']) {
      if (await page.locator(`label:has-text("${label}")`).count() === 0)
        bug(`Profil Bilgileri sekmesinde "${label}" alanı bulunamadı`);
    }
  });

  test('Profil Bilgileri güncelleme butonu görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Profil Bilgileri' }).click();
    await page.locator('label').first().waitFor({ state: 'visible', timeout: 5_000 }).catch(() => {});
    if (await page.locator('button:has-text("Güncelle"), button:has-text("Kaydet"), button[type="submit"]').count() === 0)
      bug('Profil Bilgileri sekmesinde güncelleme butonu bulunamadı');
  });

  test('Güvenlik sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Güvenlik' }).click();
    await page.waitForSelector('text=Şifre, text=Güvenlik', { timeout: 5_000 }).catch(() => {});
    if (await page.locator('text=Şifre').or(page.locator('text=Güvenlik')).count() === 0)
      bug('Güvenlik sekmesi içeriği yüklenmiyor');
  });

  test('Bildirimler sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Bildirimler' }).click();
    await page.waitForSelector('text=Bildirim', { timeout: 5_000 }).catch(() => {});
    if (await page.locator('text=Bildirim').first().count() === 0)
      bug('Bildirimler sekmesi içeriği yüklenmiyor');
  });

  test('Ödeme Yöntemleri sekmesi açılır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.getByRole('button', { name: 'Ödeme Yöntemleri' }).click();
    await page.waitForSelector('text=Kart, text=Ödeme', { timeout: 5_000 }).catch(() => {});
    if (await page.locator('text=Kart').or(page.locator('text=Ödeme')).count() === 0)
      bug('Ödeme Yöntemleri sekmesi içeriği yüklenmiyor');
  });

});
