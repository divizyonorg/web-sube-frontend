import { test, expect } from '@playwright/test';
import { bug } from './bug';

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

  test('header görünür', async ({ page }) => {
    const header = page.locator('header, [class*="header"]').first();
    await expect(header).toBeVisible({ timeout: 10_000 });
  });

  test('kullanıcı avatarı/adı görünür', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const avatar = page.locator('[class*="avatar"], [class*="user"]').first();
    if (await avatar.count() === 0) bug('Header\'da kullanıcı avatarı/adı bulunamadı');
  });

  test('bildirim alanı görünür', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const notif = page.locator('[class*="notif"], [class*="badge"]').first();
    if (await notif.count() === 0) bug('Header\'da bildirim alanı bulunamadı');
  });

  test('sidebar görünür', async ({ page }) => {
    const sidebar = page.locator('nav:has(a[href="/anasayfa"]), aside:has(a[href="/anasayfa"]), [class*="sidebar"]:has(a[href="/anasayfa"])').first();
    if (await sidebar.count() === 0) {
      bug('Sidebar navigasyon DOM\'da bulunamadı');
      return;
    }
    if (!await sidebar.isVisible()) bug('Sidebar navigasyon DOM\'da var ama görünmüyor — CSS sorunu olabilir');
  });

  test('tüm sidebar linkleri DOM\'da mevcut', async ({ page }) => {
    await page.waitForLoadState('domcontentloaded');
    for (const p of SIDEBAR_PAGES) {
      const link = page.locator(`a[href="${p.href}"]`).first();
      if (await link.count() === 0) bug(`Sidebar'da "${p.label}" (${p.href}) linki bulunamadı`);
    }
  });

  for (const p of SIDEBAR_PAGES) {
    test(`${p.label} sayfası erişilebilir (404 yok)`, async ({ page }) => {
      const response = await page.goto(p.href);
      await page.waitForLoadState('domcontentloaded');

      const status = response?.status();
      if (status && status >= 400) bug(`${p.href} → HTTP ${status} hatası`);

      if (await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0)
        bug(`${p.href} 404 sayfası gösteriyor`);

      const finalUrl = page.url();
      if (finalUrl.includes('/login') || finalUrl.includes('/Login'))
        bug(`${p.href} → Login sayfasına yönlendirdi — oturum geçersiz olabilir`);

      if (p.check) {
        if (await page.locator(`text=${p.check}`).first().count() === 0)
          bug(`${p.href} yüklendi ama "${p.check}" içeriği bulunamadı`);
      }
    });
  }

  test('VIP Danışmanlık sayfası erişilebilir', async ({ page }) => {
    const response = await page.goto('/VipDanismalikPaketleri');
    await page.waitForLoadState('domcontentloaded');
    const status = response?.status();
    if (status && status >= 400) bug(`/VipDanismalikPaketleri → HTTP ${status}`);
  });

  test('logo\'ya tıklayınca anasayfaya gidiliyor', async ({ page }) => {
    const logo = page.locator('header a, [class*="logo"] a, a:has(img[alt*="logo"]), a:has(img[alt*="Logo"]), a:has(img[alt*="Kredi"])').first();
    if (await logo.count() === 0) {
      bug('Header\'da logo linki bulunamadı');
      return;
    }
    if (!await logo.isVisible()) {
      bug('Header\'da logo linki DOM\'da var ama görünmüyor');
      return;
    }
    await logo.click();
    await page.waitForLoadState('domcontentloaded');
    const url = page.url();
    if (!url.includes('anasayfa') && !url.endsWith('/'))
      bug(`Logo tıklaması anasayfaya götürmüyor — mevcut URL: ${url}`);
  });

  test('mobil hamburger menü butonu DOM\'da mevcut', async ({ page }) => {
    const hamburger = page.locator('button[aria-label*="menü"], button[aria-label*="menu"], button[class*="hamburger"], button[class*="mobile"]').first();
    if (await hamburger.count() === 0) bug('Mobil hamburger menü butonu bulunamadı');
  });

});
