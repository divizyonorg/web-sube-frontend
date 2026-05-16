import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Finansal Profil Tamamla Akışı', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Ayarlar');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    // Finansal Profil sekmesine git
    const tab = page.getByRole('button', { name: 'Finansal Profil' });
    if (await tab.count() > 0) await tab.click();
    await page.waitForTimeout(500);
  });

  // ─── Bug 2: Sıfırla başlayan gelir değeri ─────────────────────────────────
  test('[BUG-2] Aylık net gelir "0707" gibi sıfırla başlayan değer kabul edilmemeli', async ({ page }) => {
    const gelirInput = page.locator('input[name*="Gelir"], input[id*="Gelir"], input[placeholder*="Gelir"], input[placeholder*="gelir"]').first();
    if (await gelirInput.count() === 0) {
      info('Aylık net gelir input bulunamadı — sekme içeriği farklı olabilir');
      return;
    }
    await gelirInput.fill('0707');
    await page.waitForTimeout(300);
    const val = await gelirInput.inputValue();
    // 0 ile başlayan değer ya reddedilmeli ya da otomatik düzeltilmeli
    if (val === '0707')
      bug('[BUG-2] Aylık net gelir alanı "0707" gibi sıfırla başlayan geçersiz değeri kabul ediyor');
  });

  // ─── Bug 5: Profil Bilgileri kaydedilince toast yok ───────────────────────
  test('[BUG-5] Profil Bilgileri kaydedilince başarı bildirimi gösterilmeli', async ({ page }) => {
    await page.goto('/Ayarlar');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const profilTab = page.getByRole('button', { name: 'Profil Bilgileri' });
    if (await profilTab.count() === 0) { bug('[BUG-5] Profil Bilgileri sekmesi bulunamadı'); return; }
    await profilTab.click();
    await page.waitForTimeout(500);
    const saveBtn = page.locator('button:has-text("Kaydet"), button:has-text("Güncelle"), button[type="submit"]').first();
    if (await saveBtn.count() === 0) { bug('[BUG-5] Kaydet butonu bulunamadı'); return; }
    await saveBtn.click({ force: true });
    await page.waitForTimeout(2000);
    const toast = page.locator('[class*="toast"], [class*="alert"], [class*="notification"], [class*="success"]').first();
    if (await toast.count() === 0)
      bug('[BUG-5] Profil bilgileri kaydedildiğinde başarı/hata bildirimi (toast) gösterilmiyor');
  });

  // ─── Bug 6: Finansal Profil kaydedilince toast yok ────────────────────────
  test('[BUG-6] Finansal Profil kaydedilince başarı bildirimi gösterilmeli', async ({ page }) => {
    const saveBtn = page.locator('button:has-text("Kaydet"), button[type="submit"]').first();
    if (await saveBtn.count() === 0) { bug('[BUG-6] Finansal Profil kaydet butonu bulunamadı'); return; }
    await saveBtn.click({ force: true });
    await page.waitForTimeout(2000);
    const toast = page.locator('[class*="toast"], [class*="alert"], [class*="notification"], [class*="success"]').first();
    if (await toast.count() === 0)
      bug('[BUG-6] Finansal Profil kaydedildiğinde başarı/hata bildirimi (toast) gösterilmiyor');
  });

  // ─── Bug 9: İş Bilgileri kayıt bildirimi tutarsız ─────────────────────────
  test('[BUG-9] İş Bilgileri kayıt sonrası bildirim tutarlı olmalı', async ({ page }) => {
    await page.goto('/Ayarlar/FinansalProfil').catch(() => {});
    await page.waitForLoadState('domcontentloaded');
    const isTab = page.locator('text=İş Bilgileri, text=Çalışma Durumu').first();
    if (await isTab.count() === 0) { info('[BUG-9] İş Bilgileri bölümü Ayarlar sayfasında test edilemedi'); return; }
    const saveBtn = page.locator('button:has-text("Kaydet")').first();
    if (await saveBtn.count() === 0) return;
    // 3 kez kaydet — bildirim her seferinde tutarlı gelmeli
    for (let i = 0; i < 3; i++) {
      await saveBtn.click({ force: true });
      await page.waitForTimeout(1500);
    }
    const errorToast = page.locator('text=kaydedilemedi').first();
    if (await errorToast.count() > 0)
      bug('[BUG-9] İş Bilgileri kaydında "kaydedilemedi" hatası alındı — API tutarsız davranıyor');
  });

  // ─── Bug 12: Zorunlu alanlar boş bırakılarak ilerlenebiliyor ──────────────
  test('[BUG-12] Zorunlu alanlar boş bırakılınca "Devam Et" engellemeli', async ({ page }) => {
    await page.goto('/KrediBasvurusu');
    await page.waitForLoadState('domcontentloaded');
    await page.waitForTimeout(1500);
    // Finansal profil adımını bul (Adım 2/2 Gelir Bilgileri)
    const devamBtn = page.locator('button:has-text("Devam Et"), button:has-text("Devam"), button:has-text("İleri")').first();
    if (await devamBtn.count() === 0) { info('[BUG-12] Devam Et butonu bulunamadı'); return; }
    const urlBefore = page.url();
    await devamBtn.click({ force: true });
    await page.waitForTimeout(1000);
    const urlAfter = page.url();
    const stepIndicator = page.locator('text=Adım 2').first();
    // URL değiştiyse veya adım ilerlediyse validasyon çalışmıyor demek
    if (urlAfter !== urlBefore || await stepIndicator.count() > 0)
      bug('[BUG-12] Zorunlu alanlar boş bırakılarak "Devam Et" ile sonraki adıma geçilebildi — validasyon çalışmıyor');
  });

});
