import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Destek Merkezi', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/DestekMerkezi');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/DestekMerkezi/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  test('iletişim kartları görünür (Telefon, Mail, Canlı Destek)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const label of ['Ara', 'Mail Gönder', 'Başlat']) {
      if (await page.locator(`button:has-text("${label}"), a:has-text("${label}")`).count() === 0)
        bug(`"${label}" iletişim butonu sayfada bulunamadı`);
    }
  });

  test('SSS başlığı görünür', async ({ page }) => {
    await expect(page.locator('text=Sıkça Sorulan Sorular')).toBeVisible({ timeout: 15_000 });
  });

  test('SSS kategorileri API\'den yükleniyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const count = await page.locator('button[onclick="toggleAccordion(this)"]').count();
    info(`SSS soru sayısı: ${count}`);
    if (count === 0)
      bug('SSS soruları yüklenmiyor — API\'den veri gelmiyor olabilir');
  });

  test('SSS accordion açılıp kapanıyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const firstBtn = page.locator('button[onclick="toggleAccordion(this)"]').first();
    if (await firstBtn.count() === 0) {
      bug('SSS accordion butonu bulunamadı');
      return;
    }
    const answer = firstBtn.locator('+ div');
    await expect(answer).toHaveClass(/hidden/, { timeout: 3_000 });
    await firstBtn.click();
    if (!await answer.evaluate(el => !el.classList.contains('hidden')))
      bug('SSS sorusuna tıklanınca cevap açılmıyor');
    await firstBtn.click();
    if (!await answer.evaluate(el => el.classList.contains('hidden')))
      bug('SSS sorusuna ikinci tıklamada cevap kapanmıyor');
  });

  test('SSS arama çubuğu çalışıyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const searchInput = page.locator('main').locator('input[type="search"], input[placeholder*="ara"], input[placeholder*="Ara"]').first();
    if (await searchInput.count() === 0) {
      bug('SSS arama çubuğu sayfada bulunamadı');
      return;
    }
    await searchInput.fill('kredi');
    await page.waitForTimeout(500);
  });

  test('Destek Taleplerim bölümü görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await expect(page.locator('text=Destek Taleplerim')).toBeVisible({ timeout: 10_000 });
  });

  test('"Yeni Destek Talebi Oluştur" butonu görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await expect(page.locator('button:has-text("Yeni Destek Talebi")')).toBeVisible({ timeout: 10_000 });
  });

  test('Yeni Destek Talebi modalı açılır ve form görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.locator('button:has-text("Yeni Destek Talebi")').click();
    await expect(page.locator('textarea').first()).toBeVisible({ timeout: 5_000 });
  });

  test('Destek Talebi modal kapatılabiliyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.locator('button:has-text("Yeni Destek Talebi")').click();
    await expect(page.locator('textarea').first()).toBeVisible({ timeout: 5_000 });
    const closeBtn = page.locator('button:has-text("İptal"), button[aria-label="Kapat"]').first();
    if (await closeBtn.count() === 0) {
      bug('Destek talebi modalında kapatma/iptal butonu bulunamadı');
    } else {
      await closeBtn.click();
      await expect(page.locator('textarea')).toBeHidden({ timeout: 3_000 });
    }
  });

  test('mevcut destek talepleri listeleniyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const rows = page.locator('[class*="talep"], [class*="request"], table tr').filter({ hasNot: page.locator('th') });
    const count = await rows.count();
    info(`Mevcut destek talebi sayısı: ${count}`);
    if (count === 0)
      bug('Destek talebi listesi boş — bu kullanıcıya ait talep yok olabilir');
  });

  // ─── Bug 3: Destek talebi gönderilemiyor ─────────────────────────────────
  test('[BUG-3] Destek talebi gönderilince "Bir hata oluştu" alınmamalı', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    await page.locator('button:has-text("Yeni Destek Talebi")').click();
    await expect(page.locator('textarea').first()).toBeVisible({ timeout: 5_000 });

    // Formu doldur
    await page.locator('textarea').first().fill('Otomatik test talebi — lütfen görmezden gelin');

    const subjectInput = page
      .locator('input[name*="konu"], input[name*="subject"], input[placeholder*="Konu"], input[placeholder*="Başlık"]')
      .first();
    if (await subjectInput.count() > 0)
      await subjectInput.fill('Test Talebi');

    const categorySelect = page.locator('select[name*="kategori"], select[name*="category"]').first();
    if (await categorySelect.count() > 0)
      await categorySelect.selectOption({ index: 1 });

    // Gönder
    const submitBtn = page
      .locator('button[type="submit"]:visible, button:has-text("Gönder"):visible, button:has-text("Kaydet"):visible')
      .last();
    if (await submitBtn.count() === 0) { bug('[BUG-3] Destek talebi gönder butonu bulunamadı'); return; }
    await submitBtn.click({ force: true });
    await page.waitForTimeout(2500);

    // Hata mesajı kontrolü
    const errorMsg = page.locator('text=Bir hata oluştu').or(page.locator('text=hata oluştu')).first();
    if (await errorMsg.count() > 0)
      bug('[BUG-3] Destek talebi gönderilince "Bir hata oluştu" mesajı alındı — API isteği başarısız');
  });

});
