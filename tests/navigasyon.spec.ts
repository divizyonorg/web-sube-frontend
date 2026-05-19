import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

const SIDEBAR_PAGES = [
  { label: 'Anasayfa',             href: '/anasayfa',            check: 'Tekrar Hoşgeldin' },
  { label: 'Kredi Başvurusu',      href: '/KrediBasvurusu',      check: null },
  { label: 'Kredi Raporları',      href: '/KrediRaporlari',      check: null },
  { label: 'Sana Özel Teklifler',  href: '/SanaOzelTeklifler',   check: null },
  { label: 'Kredi Danışmanı',      href: '/KrediDanismani',      check: 'Kredi Danışmanı' },
  { label: 'Destek Merkezi',       href: '/DestekMerkezi',       check: 'Destek Merkezi' },
  { label: 'Canlı Destek',         href: '/CanliDestek',         check: 'Canlı Destek' },
  { label: 'Faturalarım',          href: '/Faturalarim',         check: null },
  { label: 'Sözleşmelerim',        href: '/Sozlesmelerim',       check: null },
  { label: 'Ayarlar',              href: '/Ayarlar',             check: 'Ayarlar' },
];

test.describe('Navigasyon', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/anasayfa');
    await page.waitForLoadState('domcontentloaded');
  });

  // ─── Header ───────────────────────────────────────────────────────────────
  test('header görünür', async ({ page }) => {
    const header = page.locator('header, [class*="header"]').first();
    await expect(header).toBeVisible({ timeout: 10_000 });
  });

  test('kullanıcı avatarı/adı görünür', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const avatar = page.locator('button.ua-profile-btn').first();
    if (await avatar.count() === 0)
      bug('Header\'da kullanıcı avatarı/adı bulunamadı');
  });

  // ─── Bug 1: Header'da kullanıcı adı kısaltılıyor ─────────────────────────
  test('[BUG-1] Header\'da kullanıcı adı "..." ile kısaltılmamalı', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const nameEl = page.locator('[data-user-name]').first();
    if (await nameEl.count() === 0) {
      info('[BUG-1] Header\'da kullanıcı adı elementi bulunamadı');
      return;
    }
    const nameText = (await nameEl.textContent()) ?? '';
    info(`Header kullanıcı adı: "${nameText.trim()}"`);
    if (nameText.includes('...') || nameText.includes('…'))
      bug(`[BUG-1] Header'daki kullanıcı adı kısaltılmış: "${nameText.trim()}" — CSS text-overflow/truncate sorunu`);
  });

  test('bildirim alanı görünür', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const notif = page.locator('button:has(img[src*="bell"])').first();
    if (await notif.count() === 0)
      bug('Header\'da bildirim alanı bulunamadı');
  });

  // ─── Sidebar ──────────────────────────────────────────────────────────────
  test('sidebar görünür', async ({ page }) => {
    const sidebar = page.locator('#layout-desktop-sidebar').first();
    await expect(sidebar).toBeVisible({ timeout: 10_000 });
  });

  test('tüm sidebar linkleri DOM\'da mevcut', async ({ page }) => {
    await page.locator('#layout-desktop-sidebar').first().waitFor({ timeout: 10_000 });
    for (const p of SIDEBAR_PAGES) {
      const link = page.locator(`a[href="${p.href}"]`).first();
      if (await link.count() === 0)
        bug(`Sidebar'da "${p.label}" (${p.href}) linki bulunamadı`);
    }
  });

  // ─── Her sayfa 404 dönmesin ───────────────────────────────────────────────
  for (const p of SIDEBAR_PAGES) {
    test(`${p.label} sayfası erişilebilir (404 yok)`, async ({ page }) => {
      const response = await page.goto(p.href);
      await page.waitForLoadState('domcontentloaded');

      const status = response?.status();
      if (status && status >= 400)
        bug(`${p.href} → HTTP ${status} hatası`);

      if (await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0)
        bug(`${p.href} 404 sayfası gösteriyor`);

      const finalUrl = page.url();
      if (finalUrl.includes('/login') || finalUrl.includes('/Login'))
        bug(`${p.href} → Login sayfasına yönlendirdi — oturum geçersiz olabilir`);

      if (p.check) {
        if (await page.locator(`text=${p.check}`).count() === 0)
          bug(`${p.href} yüklendi ama "${p.check}" içeriği bulunamadı`);
      }
    });
  }

  // ─── VIP Sayfası ──────────────────────────────────────────────────────────
  test('VIP Danışmanlık sayfası erişilebilir', async ({ page }) => {
    const response = await page.goto('/VipDanismalikPaketleri');
    await page.waitForLoadState('domcontentloaded');
    const status = response?.status();
    if (status && status >= 400)
      bug(`/VipDanismalikPaketleri → HTTP ${status}`);
  });

  // ─── Logo tıklaması ───────────────────────────────────────────────────────
  test('İnteraktif Kredi logosu tıklanınca anasayfaya gidiliyor', async ({ page }) => {
    // Sol üstteki İnteraktif Kredi logosu — sidebar veya header içinde
    const logo = page
      .locator('a[href="/anasayfa"], a[href="/"], aside a img, aside a svg, [class*="logo"] a, header a img, header a svg')
      .first();
    if (await logo.count() === 0) {
      bug('Sol üstteki İnteraktif Kredi logosu linki bulunamadı');
      return;
    }
    await logo.click({ force: true });
    await page.waitForLoadState('domcontentloaded');
    const url = page.url();
    if (!url.includes('anasayfa') && !url.endsWith('/'))
      bug(`İnteraktif Kredi logosu tıklaması anasayfaya götürmüyor — mevcut URL: ${url}`);
  });

  // ─── Logout ───────────────────────────────────────────────────────────────
  test('çıkış yapılabiliyor ve login sayfasına yönlendiriyor', async ({ page }) => {
    // Logout butonu .ua-dropdown içinde — önce profil butonuna tıklayarak dropdown açılır
    const profileBtn = page.locator('button.ua-profile-btn').first();
    if (await profileBtn.count() === 0) {
      bug('Profil butonu bulunamadı — çıkış testi yapılamıyor');
      return;
    }
    await profileBtn.click();
    await page.locator('.ua-dropdown').first().waitFor({ state: 'visible', timeout: 5_000 });

    const logoutBtn = page.locator('.ua-dropdown button:has-text("Çıkış")').first();
    if (await logoutBtn.count() === 0) {
      bug('Çıkış yap butonu .ua-dropdown içinde bulunamadı');
      return;
    }

    await logoutBtn.click();
    await page.waitForLoadState('domcontentloaded', { timeout: 10_000 });

    const url = page.url();
    if (!url.includes('/login') && !url.includes('/Login'))
      bug(`Çıkış sonrası login sayfasına yönlendirilmedi — mevcut URL: ${url}`);
  });

  // ─── Mobil menü ───────────────────────────────────────────────────────────
  test('mobil hamburger menü butonu DOM\'da mevcut', async ({ page }) => {
    const hamburger = page.locator('button[aria-label*="menü"], button[class*="hamburger"], button[class*="mobile"]').first();
    if (await hamburger.count() === 0)
      bug('Mobil hamburger menü butonu bulunamadı');
  });

});
