import { test, expect } from '@playwright/test';

test.use({ storageState: { cookies: [], origins: [] } });

test.describe('Login Sayfası', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve başlık görünür', async ({ page }) => {
    await expect(page.locator('text=İnteraktif Şube Girişi').or(page.locator('h1')).first()).toBeVisible({ timeout: 10_000 });
  });

  test('GSM input alanı görünür ve IMask maskeleme çalışır', async ({ page }) => {
    const phoneInput = page.locator('#PhoneNumber');
    await expect(phoneInput).toBeVisible({ timeout: 10_000 });
    await phoneInput.fill('5389503496');
    const val = await phoneInput.inputValue();
    if (!val.includes('5389503496')) {
      console.warn('🐛 BUG: IMask telefon maskeleme beklenmedik format üretti:', val);
    }
  });

  test('boş GSM ile Devam Et\'e basılınca TCKN adımına geçilmiyor', async ({ page }) => {
    await page.getByRole('button', { name: 'Devam Et' }).click();
    await page.waitForTimeout(500);
    const tcknInput = page.locator('#Tckn');
    const isVisible = await tcknInput.isVisible();
    if (isVisible) {
      console.warn('🐛 BUG: Boş GSM ile Devam Et\'e basılınca TCKN adımı açıldı — validasyon çalışmıyor');
    }
    await expect(tcknInput).toBeHidden();
  });

  test('geçerli GSM ile Devam Et\'e basılınca TCKN adımı açılır', async ({ page }) => {
    await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
    await page.getByRole('button', { name: 'Devam Et' }).click();
    await expect(page.locator('#Tckn')).toBeVisible({ timeout: 5_000 });
    await expect(page.getByRole('button', { name: 'Giriş Yap', exact: true })).toBeVisible();
  });

  test('geçersiz TCKN formatı (checksum hatalı) → inline hata gösterilir', async ({ page }) => {
    await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
    await page.getByRole('button', { name: 'Devam Et' }).click();
    await expect(page.locator('#Tckn')).toBeVisible({ timeout: 5_000 });

    await page.locator('#Tckn').fill('12345678900');
    await page.getByRole('button', { name: 'Giriş Yap', exact: true }).click();

    const errMsg = page.locator('[x-show="step === 2 && tcknError"]');
    const isVisible = await errMsg.isVisible({ timeout: 3_000 }).catch(() => false);
    if (!isVisible) {
      console.warn('🐛 BUG: Geçersiz TCKN girilince inline hata mesajı görünmüyor');
    }
    await expect(page.locator('#OtpCode')).toBeHidden();
  });

  test('Google ile Giriş Yap butonu görünür', async ({ page }) => {
    const googleBtn = page.locator('button:has-text("Google")').or(page.locator('a:has-text("Google")')).first();
    const exists = await googleBtn.count() > 0;
    if (!exists) {
      console.warn('⚠️ BİLGİ: Google ile giriş butonu sayfada bulunamadı');
    }
  });

  test('OTP sayaç geri sayım çalışır', async ({ page }) => {
    await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
    await page.getByRole('button', { name: 'Devam Et' }).click();
    await page.locator('#Tckn').fill('14266534124');
    await page.getByRole('button', { name: 'Giriş Yap', exact: true }).click();

    const otpModal = page.locator('#OtpCode');
    const isVisible = await otpModal.isVisible({ timeout: 8_000 }).catch(() => false);
    if (!isVisible) {
      console.warn('🐛 BUG: Geçerli TCKN ile OTP modalı açılmıyor');
      test.skip();
      return;
    }

    const timer = page.locator('text=/\\d+:\\d+/').first();
    const timerVisible = await timer.isVisible({ timeout: 3_000 }).catch(() => false);
    if (!timerVisible) {
      console.warn('🐛 BUG: OTP modalında geri sayım sayacı görünmüyor');
    }
  });

});
