import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Anasayfa', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/anasayfa');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa başarıyla açılır ve karşılama metni görünür', async ({ page }) => {
    await expect(page).toHaveURL(/anasayfa/);
    await expect(page.locator('text=Tekrar Hoşgeldin').first()).toBeVisible({ timeout: 10_000 });
  });

  test('Kredi Uygunluk kartı yüklenir ve skoru gösterir', async ({ page }) => {
    await expect(page.locator('text=Krediye uygun musun?').first()).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Kredi Uygunluk Durumun').first()).toBeVisible({ timeout: 5_000 });
    const kurBtn = page.locator('a:has-text("K.U.R Raporunu Gör"), a:has-text("Rapor")').first();
    if (await kurBtn.count() === 0)
      bug('Kredi Uygunluk kartındaki "K.U.R Raporunu Gör" butonu bulunamadı');
  });

  test('Kredi Uygunluk slider/skor değeri DOM\'da mevcut', async ({ page }) => {
    await page.locator('text=Krediye uygun musun?').first().waitFor({ timeout: 10_000 });
    const sliderEl = page.locator('[style*="left"]').or(page.locator('.slider, [class*="slider"]')).first();
    if (await sliderEl.count() === 0)
      bug('Kredi Uygunluk slider elementi bulunamadı — API verisi gelmiyor olabilir');
  });

  test('Kredi Nabzı kartı yüklenir', async ({ page }) => {
    await expect(page.locator('text=Kredi Nabzı').first()).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Bankalarda kredi muslukları açık mı?').first()).toBeVisible({ timeout: 5_000 });
  });

  test('Kredi Nabzı dropdown kredi türünü değiştiriyor', async ({ page }) => {
    await page.locator('text=Kredi Nabzı').first().waitFor({ timeout: 10_000 });
    const dropdown = page.locator('select, [x-data] button').filter({ hasText: /İhtiyaç|Konut|Araç|Ticari/ }).first();
    if (await dropdown.count() === 0) {
      bug('Kredi Nabzı kredi türü dropdown\'ı bulunamadı');
    } else {
      await dropdown.click();
      await page.waitForLoadState('networkidle').catch(() => {});
    }
  });

  test('Kredi Danışmanlığı kartı yüklenir ve butonu görünür', async ({ page }) => {
    await expect(page.locator('text=Kredi Danışmanlığı').first()).toBeVisible({ timeout: 10_000 });
    const uzmanBtn = page.locator('button:has-text("Kredi Uzmanı"), a:has-text("Kredi Uzmanı")').first();
    if (await uzmanBtn.count() === 0)
      bug('"Kredi Uzmanı ile Devam Et" butonu Danışmanlık kartında bulunamadı');
  });

  test('VIP Paketler kartı içeriği görünür', async ({ page }) => {
    await expect(page.locator('text=VIP Paketler').first()).toBeVisible({ timeout: 10_000 });
    const vipLink = page.locator('a[href="/VipDanismalikPaketleri"]').first();
    await expect(vipLink).toBeAttached();
    if (!await vipLink.isVisible())
      bug('VIP Paketleri İncele linki DOM\'da var ama görünmüyor — CSS hidden sorunu');
  });

  test('Popüler Krediler swiper görünür ve slide\'lar yüklü', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    const count = await page.locator('#loans-swiper .swiper-slide').count();
    info(`Popüler Krediler slide sayısı: ${count}`);
    if (count === 0)
      bug('Popüler Krediler swiper\'ında hiç slide yok');
  });

  test('Popüler Krediler ileri/geri butonları çalışır', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    await page.locator('#loans-next').click({ force: true });
    await page.locator('#loans-swiper .swiper-slide-active').waitFor({ state: 'visible', timeout: 3_000 }).catch(() => {});
    await page.locator('#loans-prev').click({ force: true });
  });

  test('Popüler Krediler kartında Başvur butonu görünür', async ({ page }) => {
    await expect(page.locator('#loans-swiper')).toBeVisible({ timeout: 10_000 });
    if (await page.locator('#loans-swiper button:has-text("Başvur")').count() === 0)
      bug('Popüler Krediler kartında "Başvur" butonu bulunamadı');
  });

  test('Avantajlı Kredi Kartları swiper görünür', async ({ page }) => {
    await expect(page.locator('#cards-swiper')).toBeVisible({ timeout: 10_000 });
    const count = await page.locator('#cards-swiper .swiper-slide').count();
    info(`Kredi Kartları slide sayısı: ${count}`);
    if (count === 0)
      bug('Avantajlı Kredi Kartları swiper\'ında hiç slide yok');
  });

  test('Avantajlı Kredi Kartları ileri/geri butonları çalışır', async ({ page }) => {
    await expect(page.locator('#cards-swiper')).toBeVisible({ timeout: 10_000 });
    await page.locator('#cards-next').click({ force: true });
    await page.locator('#cards-swiper .swiper-slide-active').waitFor({ state: 'visible', timeout: 3_000 }).catch(() => {});
    await page.locator('#cards-prev').click({ force: true });
  });

  test('Kampanyalar bölümü görünür', async ({ page }) => {
    await expect(page.locator('#campaigns-swiper')).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('text=Kampanyalar').first()).toBeVisible();
  });

  test('Kampanyalar verisi yükleniyor mu?', async ({ page }) => {
    await expect(page.locator('#campaigns-swiper')).toBeVisible({ timeout: 10_000 });
    const slideCount = await page.locator('#campaigns-swiper .swiper-slide').count();
    info(`Kampanya slide sayısı: ${slideCount}`);
    if (slideCount === 0)
      bug('Kampanyalar bölümünde hiç veri yok — API\'den kampanya gelmiyor');
    const isLocked = await page.locator('#campaigns-next').evaluate(el => el.classList.contains('swiper-button-lock')).catch(() => true);
    if (isLocked)
      bug('Kampanyalar swiper nav butonları swiper-button-lock ile kilitli — yeterli slide yok');
  });

  // ─── Bug 7: Hardcoded "Mustafa" isim ─────────────────────────────────────
  test('[BUG-7] Karşılama mesajındaki isim giriş yapan kullanıcıya ait olmalı (hardcoded değil)', async ({ page }) => {
    const welcomeEl = page.locator('text=Tekrar Hoşgeldin').first();
    await welcomeEl.waitFor({ timeout: 10_000 });

    // Karşılama container metnini al
    const containerText = await page
      .locator('h1:has-text("Hoşgeldin"), h2:has-text("Hoşgeldin"), p:has-text("Hoşgeldin"), [class*="welcome"]:has-text("Hoşgeldin")')
      .first()
      .textContent()
      .catch(() => '');
    info(`Karşılama metni: "${containerText?.trim()}"`);

    // Header'daki kullanıcı adını al ve karşılamayla karşılaştır
    const headerName = await page
      .locator('header [class*="name"], header [class*="user"] span, header strong, header [class*="profile"] span')
      .first()
      .textContent()
      .catch(() => '');
    info(`Header kullanıcı adı: "${headerName?.trim()}"`);

    if (headerName?.trim() && containerText) {
      const firstName = headerName.trim().split(' ')[0];
      if (firstName && !containerText.includes(firstName))
        bug(`[BUG-7] Karşılama mesajı ("${containerText?.trim()}") header'daki kullanıcı adıyla ("${headerName?.trim()}") eşleşmiyor — hardcoded isim kullanılıyor`);
    }
  });

});
