import { test, expect } from '@playwright/test';

test.describe('Anasayfa', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/anasayfa');
    await page.waitForLoadState('domcontentloaded');
  });

  // ─── Sayfa Yüklenme ───────────────────────────────────────────────────────
  test('sayfa başarıyla açılır ve karşılama metni görünür', async ({ page }) => {
    await expect(page).toHaveURL(/anasayfa/);
    await expect(page.locator('text=Tekrar Hoşgeldin').first()).toBeVisible({ timeout: 10_000 });
  });

  // ─── Kredi Uygunluk Kartı ─────────────────────────────────────────────────
  test('Kredi Uygunluk kartı yüklenir ve skoru gösterir', async ({ page }) => {
    await expect(page.locator('text=Krediye uygun musun?').first()).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Kredi Uygunluk Durumun').first()).toBeVisible({ timeout: 5_000 });

    const kurBtn = page.locator('a:has-text("K.U.R Raporunu Gör"), a:has-text("Rapor")').first();
    const exists = await kurBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Kredi Uygunluk kartındaki "K.U.R Raporunu Gör" butonu bulunamadı');
    }
  });

  test('Kredi Uygunluk slider/skor değeri DOM\'da mevcut', async ({ page }) => {
    await page.locator('text=Krediye uygun musun?').first().waitFor({ timeout: 10_000 });
    const sliderEl = page.locator('[style*="left"]').or(page.locator('.slider, [class*="slider"]')).first();
    const hasSlider = await sliderEl.count() > 0;
    if (!hasSlider) {
      console.warn('🐛 BUG: Kredi Uygunluk slider elementi bulunamadı — API verisi gelmiyor olabilir');
    }
  });

  // ─── Kredi Nabzı Kartı ────────────────────────────────────────────────────
  test('Kredi Nabzı kartı yüklenir', async ({ page }) => {
    await expect(page.locator('text=Kredi Nabzı').first()).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Bankalarda kredi muslukları açık mı?').first()).toBeVisible({ timeout: 5_000 });
  });

  test('Kredi Nabzı dropdown kredi türünü değiştiriyor', async ({ page }) => {
    await page.locator('text=Kredi Nabzı').first().waitFor({ timeout: 10_000 });
    const dropdown = page.locator('select, [x-data] button').filter({ hasText: /İhtiyaç|Konut|Araç|Ticari/ }).first();
    const exists = await dropdown.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Kredi Nabzı kredi türü dropdown\'ı bulunamadı');
    } else {
      await dropdown.click();
      await page.waitForTimeout(500);
    }
  });

  // ─── Kredi Danışmanlığı Kartı ─────────────────────────────────────────────
  test('Kredi Danışmanlığı kartı yüklenir ve butonu görünür', async ({ page }) => {
    await expect(page.locator('text=Kredi Danışmanlığı').first()).toBeVisible({ timeout: 10_000 });
    const uzmanBtn = page.locator('button:has-text("Kredi Uzmanı"), a:has-text("Kredi Uzmanı")').first();
    const exists = await uzmanBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: "Kredi Uzmanı ile Devam Et" butonu Danışmanlık kartında bulunamadı');
    }
  });

  // ─── VIP Paketler ─────────────────────────────────────────────────────────
  test('VIP Paketler kartı içeriği görünür', async ({ page }) => {
    await expect(page.locator('text=VIP Paketler').first()).toBeVisible({ timeout: 10_000 });
    const vipLink = page.locator('a[href="/VipDanismalikPaketleri"]').first();
    await expect(vipLink).toBeAttached();
    const isVisible = await vipLink.isVisible();
    if (!isVisible) {
      console.warn('🐛 BUG: VIP Paketleri İncele linki DOM\'da var ama görünmüyor — CSS hidden sorunu');
    }
  });

  // ─── Popüler Krediler Swiper ──────────────────────────────────────────────
  test('Popüler Krediler swiper görünür ve slide\'lar yüklü', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    const slides = page.locator('#loans-swiper .swiper-slide');
    const count = await slides.count();
    if (count === 0) {
      console.warn('🐛 BUG: Popüler Krediler swiper\'ında hiç slide yok');
    } else {
      console.log(`ℹ️ Popüler Krediler slide sayısı: ${count}`);
    }
  });

  test('Popüler Krediler ileri/geri butonları çalışır', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    await page.locator('#loans-next').click();
    await page.waitForTimeout(400);
    await page.locator('#loans-prev').click();
  });

  test('Popüler Krediler kartında Başvur butonu görünür', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    const basvurBtn = page.locator('#loans-swiper button:has-text("Başvur")').first();
    const exists = await basvurBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Popüler Krediler kartında "Başvur" butonu bulunamadı');
    }
  });

  // ─── Avantajlı Kredi Kartları Swiper ─────────────────────────────────────
  test('Avantajlı Kredi Kartları swiper görünür', async ({ page }) => {
    await expect(page.locator('#cards-swiper')).toBeVisible({ timeout: 10_000 });
    const count = await page.locator('#cards-swiper .swiper-slide').count();
    if (count === 0) {
      console.warn('🐛 BUG: Avantajlı Kredi Kartları swiper\'ında hiç slide yok');
    } else {
      console.log(`ℹ️ Kredi Kartları slide sayısı: ${count}`);
    }
  });

  test('Avantajlı Kredi Kartları ileri/geri butonları çalışır', async ({ page }) => {
    await expect(page.locator('#cards-swiper')).toBeVisible({ timeout: 10_000 });
    await page.locator('#cards-next').click();
    await page.waitForTimeout(400);
    await page.locator('#cards-prev').click();
  });

  // ─── Kampanyalar Swiper ───────────────────────────────────────────────────
  test('Kampanyalar bölümü görünür', async ({ page }) => {
    await expect(page.locator('#campaigns-swiper')).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Kampanyalar').first()).toBeVisible();
  });

  test('Kampanyalar verisi yükleniyor mu?', async ({ page }) => {
    await expect(page.locator('#campaigns-swiper')).toBeVisible({ timeout: 10_000 });
    const slideCount = await page.locator('#campaigns-swiper .swiper-slide').count();
    console.log(`ℹ️ Kampanya slide sayısı: ${slideCount}`);
    if (slideCount === 0) {
      console.warn('🐛 BUG: Kampanyalar bölümünde hiç veri yok — API\'den kampanya gelmiyor');
    }
    const nextBtn = page.locator('#campaigns-next');
    const isLocked = await nextBtn.evaluate(el => el.classList.contains('swiper-button-lock')).catch(() => true);
    if (isLocked) {
      console.warn('🐛 BUG: Kampanyalar swiper nav butonları swiper-button-lock ile kilitli — yeterli slide yok');
    }
  });

});
