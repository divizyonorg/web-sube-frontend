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

  // ─── Bug 2: Sıfırla başlayan gelir değeri + harf girişi ──────────────────
  test('[BUG-2] Aylık net gelir — sıfırla başlayan değer kabul edilmemeli', async ({ page }) => {
    const gelirInput = page
      .locator('input[type="number"], input[x-model*="gelir" i], input[name*="Gelir"], input[placeholder*="Gelir" i]')
      .first();
    if (await gelirInput.count() === 0) {
      info('Aylık net gelir input bulunamadı — sekme içeriği farklı olabilir');
      return;
    }

    // "0707" gibi sıfırla başlayan değer
    await gelirInput.fill('0707');
    await gelirInput.press('Tab');
    await page.waitForTimeout(300);
    const val0 = await gelirInput.inputValue();
    info(`Gelir — "0707" girince: "${val0}"`);
    if (val0 === '0707')
      bug('[BUG-2] Aylık net gelir alanı "0707" gibi sıfırla başlayan değeri kabul ediyor — başa 0 girilememelidir');

    // Tek sıfır
    await gelirInput.fill('0');
    await gelirInput.press('Tab');
    await page.waitForTimeout(200);
    const val1 = await gelirInput.inputValue();
    info(`Gelir — tek "0" girince: "${val1}"`);
    if (val1 === '0')
      bug('[BUG-2] Aylık net gelir alanına "0" girilebiliyor — tutar 0 olamaz');

    // Harf / string girişi
    await gelirInput.fill('');
    await gelirInput.type('bin');
    await page.waitForTimeout(200);
    const valStr = await gelirInput.inputValue();
    info(`Gelir — "bin" type() girince: "${valStr}"`);
    if (/[a-zA-ZığüşöçĞÜŞÖÇİ]/.test(valStr))
      bug(`[BUG-2] Aylık net gelir alanına harf girilebiliyor: "${valStr}" — sadece sayı kabul edilmeli`);
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
    const toast5 = page.locator('[data-toast]').first();
    const appeared5 = await toast5.waitFor({ state: 'attached', timeout: 8_000 }).then(() => true).catch(() => false);
    if (!appeared5)
      bug('[BUG-5] Profil bilgileri kaydedildiğinde başarı/hata bildirimi (toast) gösterilmiyor');
  });

  // ─── Bug 6: Finansal Profil kaydedilince toast yok ────────────────────────
  test('[BUG-6] Finansal Profil kaydedilince başarı bildirimi gösterilmeli', async ({ page }) => {
    const saveBtn = page.locator('button:has-text("Kaydet"), button[type="submit"]').first();
    if (await saveBtn.count() === 0) { bug('[BUG-6] Finansal Profil kaydet butonu bulunamadı'); return; }
    await saveBtn.click({ force: true });
    const toast6 = page.locator('[data-toast]').first();
    const appeared6 = await toast6.waitFor({ state: 'attached', timeout: 8_000 }).then(() => true).catch(() => false);
    if (!appeared6)
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
