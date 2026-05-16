import { test, expect } from '@playwright/test';
import { bug } from './bug';

test.use({ storageState: { cookies: [], origins: [] } });

test.describe('Register Sayfası', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/register');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve form görünür', async ({ page }) => {
    const heading = page.locator('h1').or(page.locator('text=Hemen Üye Ol')).first();
    await expect(heading).toBeVisible({ timeout: 10_000 });
  });

  test('tüm form alanları mevcut', async ({ page }) => {
    const fields = [
      { label: 'Ad',      id: 'FirstName' },
      { label: 'Soyad',   id: 'LastName' },
      { label: 'E-posta', id: 'Email' },
      { label: 'TCKN',    id: 'Tckn' },
      { label: 'Telefon', id: 'PhoneNumber' },
    ];
    for (const field of fields) {
      const el = page.locator(`#${field.id}`);
      if (await el.count() === 0) {
        bug(`${field.label} (#${field.id}) alanı sayfada bulunamadı`);
      } else {
        await expect(el).toBeVisible({ timeout: 5_000 });
      }
    }
  });

  test('telefon doğrula butonu telefon girilmeden devre dışı', async ({ page }) => {
    const dogrulaBtn = page.locator('button:has-text("Doğrula")').first();
    await expect(dogrulaBtn).toBeVisible({ timeout: 10_000 });
    if (!await dogrulaBtn.isDisabled())
      bug('Telefon girilmeden Doğrula butonu aktif — validasyon çalışmıyor');
  });

  test('telefon girilince Doğrula butonu aktifleşir', async ({ page }) => {
    await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
    await page.waitForTimeout(300);
    if (!await page.locator('button:has-text("Doğrula")').first().isEnabled())
      bug('Telefon girilince Doğrula butonu aktifleşmiyor');
  });

  test('Üye Ol butonu telefon doğrulanmadan devre dışı', async ({ page }) => {
    const uyeOlBtn = page.getByRole('button', { name: 'Üye Ol' });
    await expect(uyeOlBtn).toBeVisible({ timeout: 10_000 });
    if (!await uyeOlBtn.isDisabled())
      bug('Telefon doğrulanmadan Üye Ol butonu aktif');
  });

  test('onay kutucukları tıklanabilir', async ({ page }) => {
    const checkboxes = page.locator('input[type="checkbox"]');
    if (await checkboxes.count() === 0) {
      bug('Onay kutucukları sayfada bulunamadı');
      return;
    }
    await expect(checkboxes.first()).toBeVisible();
    await checkboxes.first().click({ force: true });
    if (!await checkboxes.first().isChecked())
      bug('Onay kutucuğuna tıklayınca işaretlenmiyor');
  });

  test('Açık Rıza Metni modalı açılır', async ({ page }) => {
    const acikRizaLink = page.locator('text=Açık Rıza').first();
    if (await acikRizaLink.count() === 0) {
      bug('Açık Rıza Metni linki sayfada bulunamadı');
      return;
    }
    await acikRizaLink.click();
    await page.waitForTimeout(500);
    const modal = page.locator('[role="dialog"], .modal, [x-show]').filter({ hasText: 'Açık Rıza' }).first();
    if (!await modal.isVisible().catch(() => false))
      bug('Açık Rıza Metni linkine tıklanınca modal açılmıyor');
  });

  test('Aydınlatma Metni modalı açılır', async ({ page }) => {
    const aydinlatmaLink = page.locator('text=Aydınlatma').first();
    if (await aydinlatmaLink.count() === 0) {
      bug('Aydınlatma Metni linki sayfada bulunamadı');
      return;
    }
    await aydinlatmaLink.click();
    await page.waitForTimeout(500);
    const modal = page.locator('[role="dialog"], .modal, [x-show]').filter({ hasText: /[Aa]ydınlatma/ }).first();
    if (!await modal.isVisible().catch(() => false))
      bug('Aydınlatma Metni linkine tıklanınca modal açılmıyor');
  });

});
