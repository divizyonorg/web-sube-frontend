import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

// Ödeme ekranı kredi başvurusu son adımında çıkar.
// Bu testler ödeme UI'ının HTML attribute'larını ve davranışlarını kontrol eder.
test.describe('Ödeme Ekranı', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediBasvurusu');
    await page.waitForLoadState('domcontentloaded');
    await page.waitForTimeout(1500);
  });

  // ─── Bug 17: CVV 4 hane kabul ediyor ──────────────────────────────────────
  test('[BUG-17] CVV alanı maksimum 3 karakter olmalı', async ({ page }) => {
    // Ödeme adımına ulaşmayı dene
    const cvvInput = page.locator('input[name*="cvv" i], input[id*="cvv" i], input[placeholder*="CVV"], input[placeholder*="cvv"]').first();
    if (await cvvInput.count() === 0) {
      info('[BUG-17] CVV input şu an görünmüyor — ödeme adımına ulaşmak için önceki adımların tamamlanması gerekiyor');
      return;
    }
    const maxLen = await cvvInput.getAttribute('maxlength');
    if (!maxLen || parseInt(maxLen) > 3)
      bug(`[BUG-17] CVV alanı maxlength="${maxLen}" — 3 olmalı, 4+ karakter girişine izin veriyor`);
    // Doğrudan 4 karakter yazmayı dene
    await cvvInput.fill('3456');
    const val = await cvvInput.inputValue();
    if (val.length > 3)
      bug(`[BUG-17] CVV alanına 4 karakter girilebiliyor: "${val}" — maksimum 3 olmalı`);
  });

  // ─── Bug 13 & 15: Kampanya kodu bildirimi birikmesi ───────────────────────
  test('[BUG-13/15] Geçersiz kampanya kodu bildirimi birikmemeli', async ({ page }) => {
    const couponInput = page.locator('input[name*="kampanya" i], input[name*="coupon" i], input[placeholder*="Kampanya"], input[placeholder*="kampanya"]').first();
    const uygulaBtn  = page.locator('button:has-text("Uygula")').first();
    if (await couponInput.count() === 0 || await uygulaBtn.count() === 0) {
      info('[BUG-13/15] Kampanya kodu alanı şu an görünmüyor — ödeme adımına ulaşılması gerekiyor');
      return;
    }
    // 3 kez geçersiz kod gir
    for (let i = 0; i < 3; i++) {
      await couponInput.fill(`GECERSIZ${i}`);
      await uygulaBtn.click();
      await page.waitForTimeout(500);
    }
    // 3'ten fazla bildirim birikmiş mi?
    const toastCount = await page.locator('[class*="toast"], [class*="notification"]').count();
    info(`Kampanya kodu toast sayısı: ${toastCount}`);
    if (toastCount > 1)
      bug(`[BUG-13/15] Kampanya kodu bildirimi birikmiyor — ${toastCount} toast aynı anda gösteriliyor, yalnızca 1 olmalı`);
  });

  // ─── Bug 14: Boş vs geçersiz kampanya kodu farklı mesajlar ───────────────
  test('[BUG-14] Boş ve geçersiz kampanya kodu aynı türde hata mesajı vermeli', async ({ page }) => {
    const couponInput = page.locator('input[name*="kampanya" i], input[name*="coupon" i], input[placeholder*="Kampanya"]').first();
    const uygulaBtn  = page.locator('button:has-text("Uygula")').first();
    if (await couponInput.count() === 0 || await uygulaBtn.count() === 0) {
      info('[BUG-14] Kampanya kodu alanı şu an görünmüyor');
      return;
    }
    // Boş bırak
    await couponInput.fill('');
    await uygulaBtn.click();
    await page.waitForTimeout(500);
    const emptyMsg = await page.locator('[class*="toast"], [class*="notification"]').first().textContent().catch(() => '');
    // Geçersiz kod gir
    await couponInput.fill('YANLISKOD123');
    await uygulaBtn.click();
    await page.waitForTimeout(500);
    const invalidMsg = await page.locator('[class*="toast"], [class*="notification"]').first().textContent().catch(() => '');
    info(`Boş kod mesajı: "${emptyMsg}" | Geçersiz kod mesajı: "${invalidMsg}"`);
    if (emptyMsg && invalidMsg && emptyMsg !== invalidMsg)
      bug(`[BUG-14] Boş ve geçersiz kampanya kodu için farklı hata mesajları: "${emptyMsg}" vs "${invalidMsg}" — tutarlı olmalı`);
  });

  // ─── Bug 16: Kart bilgisi hatası da birikmeli ─────────────────────────────
  test('[BUG-16] Hatalı kart bilgisi bildirimi birikmemeli', async ({ page }) => {
    const cardInput = page.locator('input[name*="kart" i], input[name*="card" i], input[placeholder*="Kart"]').first();
    const odeBtn    = page.locator('button:has-text("Öde"), button:has-text("Ödeme Yap")').first();
    if (await cardInput.count() === 0 || await odeBtn.count() === 0) {
      info('[BUG-16] Kart bilgisi alanı şu an görünmüyor — ödeme adımına ulaşılması gerekiyor');
      return;
    }
    for (let i = 0; i < 3; i++) {
      await cardInput.fill('0000000000000000');
      await odeBtn.click({ force: true });
      await page.waitForTimeout(800);
    }
    const toastCount = await page.locator('[class*="toast"], [class*="notification"]').count();
    if (toastCount > 1)
      bug(`[BUG-16] Hatalı kart bilgisi bildirimi birikmiyor — ${toastCount} toast aynı anda gösteriliyor`);
  });

});
