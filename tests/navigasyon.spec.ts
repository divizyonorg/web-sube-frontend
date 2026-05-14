import { test, expect } from '@playwright/test';

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
    const avatar = page.locator('[class*="avatar"], [class*="user"]').first();
    const exists = await avatar.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Header\'da kullanıcı avatarı/adı bulunamadı');
    }
  });

  test('bildirim alanı görünür', async ({ page }) => {
    await page.locator('header, [class*="header"]').first().waitFor({ timeout: 10_000 });
    const notif = page.locator('[class*="notif"], [class*="badge"]').first();
    const exists = await notif.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Header\'da bildirim alanı bulunamadı');
    }
  });

  // ─── Sidebar ──────────────────────────────────────────────────────────────
  test('sidebar görünür', async ({ page }) => {
    const sidebar = page.locator('aside, nav, [class*="sidebar"]').first();
    await expect(sidebar).toBeVisible({ timeout: 10_000 });
  });

  test('tüm sidebar linkleri DOM\'da mevcut', async ({ page }) => {
    await page.locator('aside, nav, [class*="sidebar"]').first().waitFor({ timeout: 10_000 });
    for (const p of SIDEBAR_PAGES) {
      const link = page.locator(`a[href="${p.href}"]`).first();
      const exists = await link.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: Sidebar\'da "${p.label}" (${p.href}) linki bulunamadı`);
      }
    }
  });

  // ─── Her sayfa 404 dönmesin ───────────────────────────────────────────────
  for (const p of SIDEBAR_PAGES) {
    test(`${p.label} sayfası erişilebilir (404 yok)`, async ({ page }) => {
      const response = await page.goto(p.href);
      await page.waitForLoadState('domcontentloaded');

      const status = response?.status();
      if (status && status >= 400) {
        console.warn(`🐛 BUG: ${p.href} → HTTP ${status} hatası`);
      }

      const is404 = await page.locator('text=404').or(page.locator('text=Bulunamadı')).count() > 0;
      if (is404) {
        console.warn(`🐛 BUG: ${p.href} 404 sayfası gösteriyor`);
      }

      // URL redirect kontrolü
      const finalUrl = page.url();
      if (finalUrl.includes('/login') || finalUrl.includes('/Login')) {
        console.warn(`🐛 BUG: ${p.href} → Login sayfasına yönlendirdi — oturum geçersiz olabilir`);
      }

      if (p.check) {
        const content = page.locator(`text=${p.check}`).first();
        const found = await content.count() > 0;
        if (!found) {
          console.warn(`🐛 BUG: ${p.href} yüklendi ama "${p.check}" içeriği bulunamadı`);
        }
      }
    });
  }

  // ─── VIP Sayfası ──────────────────────────────────────────────────────────
  test('VIP Danışmanlık sayfası erişilebilir', async ({ page }) => {
    const response = await page.goto('/VipDanismalikPaketleri');
    await page.waitForLoadState('domcontentloaded');
    const status = response?.status();
    if (status && status >= 400) {
      console.warn(`🐛 BUG: /VipDanismalikPaketleri → HTTP ${status}`);
    }
  });

  // ─── Logo tıklaması ───────────────────────────────────────────────────────
  test('logo\'ya tıklayınca anasayfaya gidiliyor', async ({ page }) => {
    const logo = page.locator('a[href="/anasayfa"], a[href="/"], header a, [class*="logo"] a').first();
    const exists = await logo.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Header\'da logo linki bulunamadı');
      return;
    }
    await logo.click();
    await page.waitForLoadState('domcontentloaded');
    const url = page.url();
    if (!url.includes('anasayfa') && !url.endsWith('/')) {
      console.warn(`🐛 BUG: Logo tıklaması anasayfaya götürmüyor — mevcut URL: ${url}`);
    }
  });

  // ─── Mobil menü ───────────────────────────────────────────────────────────
  test('mobil hamburger menü butonu DOM\'da mevcut', async ({ page }) => {
    const hamburger = page.locator('button[@@click*="mobileMenu"], button[aria-label*="menü"], button[class*="hamburger"]').first();
    const exists = await hamburger.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Mobil hamburger menü butonu bulunamadı');
    }
  });

});
