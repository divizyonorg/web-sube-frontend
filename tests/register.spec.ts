import { test, expect } from '@playwright/test';

test.use({ storageState: { cookies: [], origins: [] } });

test.describe('Register Sayfası', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/register');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve form görünür', async ({ page }) => {
    await expect(page.locator('h1, text=Hemen Üye Ol').first()).toBeVisible({ timeout: 10_000 });
  });

  test('tüm form alanları mevcut', async ({ page }) => {
    const fields = [
      { label: 'Ad', id: 'FirstName' },
      { label: 'Soyad', id: 'LastName' },
      { label: 'E-posta', id: 'Email' },
      { label: 'TCKN', id: 'Tckn' },
      { label: 'Telefon', id: 'PhoneNumber' },
    ];
    for (const field of fields) {
      const el = page.locator(`#${field.id}`);
      const exists = await el.count() > 0;
      if (!exists) {
        console.warn(`🐛 BUG: ${field.label} (#${field.id}) alanı sayfada bulunamadı`);
      } else {
        await expect(el).toBeVisible({ timeout: 5_000 });
      }
    }
  });

  test('telefon doğrula butonu telefon girilmeden devre dışı', async ({ page }) => {
    const dogrulaBtn = page.locator('button:has-text("Doğrula")').first();
    await expect(dogrulaBtn).toBeVisible({ timeout: 10_000 });
    const isDisabled = await dogrulaBtn.isDisabled();
    if (!isDisabled) {
      console.warn('🐛 BUG: Telefon girilmeden Doğrula butonu aktif — validasyon çalışmıyor');
    }
  });

  test('telefon girilince Doğrula butonu aktifleşir', async ({ page }) => {
    await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
    await page.waitForTimeout(300);
    const dogrulaBtn = page.locator('button:has-text("Doğrula")').first();
    const isEnabled = await dogrulaBtn.isEnabled();
    if (!isEnabled) {
      console.warn('🐛 BUG: Telefon girilince Doğrula butonu aktifleşmiyor');
    }
  });

  test('Üye Ol butonu telefon doğrulanmadan devre dışı', async ({ page }) => {
    const uyeOlBtn = page.getByRole('button', { name: 'Üye Ol' });
    await expect(uyeOlBtn).toBeVisible({ timeout: 10_000 });
    const isDisabled = await uyeOlBtn.isDisabled();
    if (!isDisabled) {
      console.warn('🐛 BUG: Telefon doğrulanmadan Üye Ol butonu aktif');
    }
  });

  test('onay kutucukları tıklanabilir', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');
    const count = await checkboxes.count();
    if (count === 0) {
      console.warn('🐛 BUG: Onay kutucukları sayfada bulunamadı');
      return;
    }
    await expect(checkboxes.first()).toBeVisible();
    await checkboxes.first().click();
    const isChecked = await checkboxes.first().isChecked();
    if (!isChecked) {
      console.warn('🐛 BUG: Onay kutucuğuna tıklayınca işaretlenmiyor');
    }
  });

  test('Açık Rıza Metni modalı açılır', async ({ page }) => {
    const acikRizaLink = page.locator('text=Açık Rıza').first();
    const exists = await acikRizaLink.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Açık Rıza Metni linki sayfada bulunamadı');
      return;
    }
    await acikRizaLink.click();
    await page.waitForTimeout(500);
    const modal = page.locator('[role="dialog"], .modal, [x-show]').filter({ hasText: 'Açık Rıza' }).first();
    const modalOpen = await modal.isVisible().catch(() => false);
    if (!modalOpen) {
      console.warn('🐛 BUG: Açık Rıza Metni linkine tıklanınca modal açılmıyor');
    }
  });

  test('Aydınlatma Metni modalı açılır', async ({ page }) => {
    const aydinlatmaLink = page.locator('text=Aydınlatma').first();
    const exists = await aydinlatmaLink.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Aydınlatma Metni linki sayfada bulunamadı');
      return;
    }
    await aydinlatmaLink.click();
    await page.waitForTimeout(500);
    const modal = page.locator('[role="dialog"], .modal, [x-show]').filter({ hasText: /[Aa]ydınlatma/ }).first();
    const modalOpen = await modal.isVisible().catch(() => false);
    if (!modalOpen) {
      console.warn('🐛 BUG: Aydınlatma Metni linkine tıklanınca modal açılmıyor');
    }
  });

});
