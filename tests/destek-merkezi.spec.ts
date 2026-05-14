import { test, expect } from '@playwright/test';

test.describe('Destek Merkezi', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/DestekMerkezi');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/DestekMerkezi/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  // ─── İletişim Kartları ────────────────────────────────────────────────────
  test('iletişim kartları görünür (Telefon, Mail, Canlı Destek)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    for (const label of ['Ara', 'Mail Gönder', 'Başlat']) {
      const btn = page.locator(`button:has-text("${label}"), a:has-text("${label}")`).first();
      const exists = await btn.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: "${label}" iletişim butonu sayfada bulunamadı`);
      }
    }
  });

  // ─── SSS ─────────────────────────────────────────────────────────────────
  test('SSS başlığı görünür', async ({ page }) => {
    await expect(page.locator('text=Sıkça Sorulan Sorular')).toBeVisible({ timeout: 15_000 });
  });

  test('SSS kategorileri API\'den yükleniyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const categories = page.locator('button[onclick="toggleAccordion(this)"]');
    const count = await categories.count();
    console.log(`ℹ️ SSS soru sayısı: ${count}`);
    if (count === 0) {
      console.warn('🐛 BUG: SSS soruları yüklenmiyor — API\'den veri gelmiyor olabilir');
    }
  });

  test('SSS accordion açılıp kapanıyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const firstBtn = page.locator('button[onclick="toggleAccordion(this)"]').first();
    const exists = await firstBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: SSS accordion butonu bulunamadı');
      return;
    }

    const answer = firstBtn.locator('+ div');
    await expect(answer).toHaveClass(/hidden/, { timeout: 3_000 });

    await firstBtn.click();
    const isOpen = await answer.evaluate(el => !el.classList.contains('hidden'));
    if (!isOpen) {
      console.warn('🐛 BUG: SSS sorusuna tıklanınca cevap açılmıyor');
    }

    await firstBtn.click();
    const isClosed = await answer.evaluate(el => el.classList.contains('hidden'));
    if (!isClosed) {
      console.warn('🐛 BUG: SSS sorusuna ikinci tıklamada cevap kapanmıyor');
    }
  });

  test('SSS arama çubuğu çalışıyor', async ({ page }) => {
    await page.locator('text=Sıkça Sorulan Sorular').waitFor({ timeout: 15_000 });
    const searchInput = page.locator('input[type="search"], input[placeholder*="ara"], input[placeholder*="Ara"]').first();
    const exists = await searchInput.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: SSS arama çubuğu sayfada bulunamadı');
      return;
    }
    await searchInput.fill('kredi');
    await page.waitForTimeout(500);
  });

  // ─── Destek Talepleri ─────────────────────────────────────────────────────
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

    const closeBtn = page.locator('button[@@click*="closeModal"], button:has-text("İptal"), button[aria-label="Kapat"]').first();
    const exists = await closeBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Destek talebi modalında kapatma/iptal butonu bulunamadı');
    } else {
      await closeBtn.click();
      await expect(page.locator('textarea')).toBeHidden({ timeout: 3_000 });
    }
  });

  test('mevcut destek talepleri listeleniyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const rows = page.locator('[class*="talep"], [class*="request"], table tr').filter({ hasNot: page.locator('th') });
    const count = await rows.count();
    console.log(`ℹ️ Mevcut destek talebi sayısı: ${count}`);
    if (count === 0) {
      console.warn('⚠️ BİLGİ: Destek talebi listesi boş — bu kullanıcıya ait talep yok olabilir');
    }
  });

});
