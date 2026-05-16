# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: login.spec.ts >> Login Sayfası >> boş GSM ile Devam Et'e basılınca TCKN adımına geçilmiyor
- Location: tests\login.spec.ts:26:7

# Error details

```
Test timeout of 30000ms exceeded.
```

```
Error: locator.click: Test timeout of 30000ms exceeded.
Call log:
  - waiting for getByRole('button', { name: 'Devam Et' })
    - locator resolved to <button type="button" class="flex flex-row items-center py-3 px-0 bg-[#0056B3] shadow-[0_4px_6px_rgba(0,0,0,0.2)] rounded-[40px] hover:bg-[#003F75]">…</button>
  - attempting click action
    2 × waiting for element to be visible, enabled and stable
      - element is visible, enabled and stable
      - scrolling into view if needed
      - done scrolling
      - <form method="post" id="loginForm" class="flex flex-col gap-4">…</form> intercepts pointer events
    - retrying click action
    - waiting 20ms
    2 × waiting for element to be visible, enabled and stable
      - element is visible, enabled and stable
      - scrolling into view if needed
      - done scrolling
      - <form method="post" id="loginForm" class="flex flex-col gap-4">…</form> intercepts pointer events
    - retrying click action
      - waiting 100ms
    59 × waiting for element to be visible, enabled and stable
       - element is visible, enabled and stable
       - scrolling into view if needed
       - done scrolling
       - <form method="post" id="loginForm" class="flex flex-col gap-4">…</form> intercepts pointer events
     - retrying click action
       - waiting 500ms

```

# Page snapshot

```yaml
- generic [ref=e2]:
  - img "İnteraktif Kredi" [ref=e4]
  - generic [ref=e6]:
    - img "İnteraktif Kredi" [ref=e7]
    - paragraph [ref=e8]: Bireysel
    - heading "İnteraktif Şube Girişi" [level=1] [ref=e9]
    - generic [ref=e11]:
      - generic [ref=e13]:
        - generic: GSM
        - textbox "GSM" [ref=e14]
      - generic:
        - button "Devam Et":
          - generic: Devam Et
      - generic [ref=e16] [cursor=pointer]:
        - checkbox
        - generic [ref=e18]: Beni Hatırla!
      - link "Henüz üye değil misin?" [ref=e21] [cursor=pointer]:
        - /url: /register
      - button "Google ile Giriş Yap" [ref=e22] [cursor=pointer]:
        - img [ref=e23]
        - text: Google ile Giriş Yap
```

# Test source

```ts
  1  | import { test, expect } from '@playwright/test';
  2  | 
  3  | test.use({ storageState: { cookies: [], origins: [] } });
  4  | 
  5  | test.describe('Login Sayfası', () => {
  6  | 
  7  |   test.beforeEach(async ({ page }) => {
  8  |     await page.goto('/login');
  9  |     await page.waitForLoadState('domcontentloaded');
  10 |   });
  11 | 
  12 |   test('sayfa yüklenir ve başlık görünür', async ({ page }) => {
  13 |     await expect(page.locator('text=İnteraktif Şube Girişi').or(page.locator('h1')).first()).toBeVisible({ timeout: 10_000 });
  14 |   });
  15 | 
  16 |   test('GSM input alanı görünür ve IMask maskeleme çalışır', async ({ page }) => {
  17 |     const phoneInput = page.locator('#PhoneNumber');
  18 |     await expect(phoneInput).toBeVisible({ timeout: 10_000 });
  19 |     await phoneInput.fill('5389503496');
  20 |     const val = await phoneInput.inputValue();
  21 |     if (!val.includes('5389503496')) {
  22 |       console.warn('🐛 BUG: IMask telefon maskeleme beklenmedik format üretti:', val);
  23 |     }
  24 |   });
  25 | 
  26 |   test('boş GSM ile Devam Et\'e basılınca TCKN adımına geçilmiyor', async ({ page }) => {
> 27 |     await page.getByRole('button', { name: 'Devam Et' }).click();
     |                                                          ^ Error: locator.click: Test timeout of 30000ms exceeded.
  28 |     await page.waitForTimeout(500);
  29 |     const tcknInput = page.locator('#Tckn');
  30 |     const isVisible = await tcknInput.isVisible();
  31 |     if (isVisible) {
  32 |       console.warn('🐛 BUG: Boş GSM ile Devam Et\'e basılınca TCKN adımı açıldı — validasyon çalışmıyor');
  33 |     }
  34 |     await expect(tcknInput).toBeHidden();
  35 |   });
  36 | 
  37 |   test('geçerli GSM ile Devam Et\'e basılınca TCKN adımı açılır', async ({ page }) => {
  38 |     await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
  39 |     await page.getByRole('button', { name: 'Devam Et' }).click();
  40 |     await expect(page.locator('#Tckn')).toBeVisible({ timeout: 5_000 });
  41 |     await expect(page.getByRole('button', { name: 'Giriş Yap', exact: true })).toBeVisible();
  42 |   });
  43 | 
  44 |   test('geçersiz TCKN formatı (checksum hatalı) → inline hata gösterilir', async ({ page }) => {
  45 |     await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
  46 |     await page.getByRole('button', { name: 'Devam Et' }).click();
  47 |     await expect(page.locator('#Tckn')).toBeVisible({ timeout: 5_000 });
  48 | 
  49 |     await page.locator('#Tckn').fill('12345678900');
  50 |     await page.getByRole('button', { name: 'Giriş Yap', exact: true }).click();
  51 | 
  52 |     const errMsg = page.locator('[x-show="step === 2 && tcknError"]');
  53 |     const isVisible = await errMsg.isVisible({ timeout: 3_000 }).catch(() => false);
  54 |     if (!isVisible) {
  55 |       console.warn('🐛 BUG: Geçersiz TCKN girilince inline hata mesajı görünmüyor');
  56 |     }
  57 |     await expect(page.locator('#OtpCode')).toBeHidden();
  58 |   });
  59 | 
  60 |   test('Google ile Giriş Yap butonu görünür', async ({ page }) => {
  61 |     const googleBtn = page.locator('button:has-text("Google")').or(page.locator('a:has-text("Google")')).first();
  62 |     const exists = await googleBtn.count() > 0;
  63 |     if (!exists) {
  64 |       console.warn('⚠️ BİLGİ: Google ile giriş butonu sayfada bulunamadı');
  65 |     }
  66 |   });
  67 | 
  68 |   test('OTP sayaç geri sayım çalışır', async ({ page }) => {
  69 |     await page.locator('#PhoneNumber').fill('+90 538 950 34 96');
  70 |     await page.getByRole('button', { name: 'Devam Et' }).click();
  71 |     await page.locator('#Tckn').fill('14266534124');
  72 |     await page.getByRole('button', { name: 'Giriş Yap', exact: true }).click();
  73 | 
  74 |     const otpModal = page.locator('#OtpCode');
  75 |     const isVisible = await otpModal.isVisible({ timeout: 8_000 }).catch(() => false);
  76 |     if (!isVisible) {
  77 |       console.warn('🐛 BUG: Geçerli TCKN ile OTP modalı açılmıyor');
  78 |       test.skip();
  79 |       return;
  80 |     }
  81 | 
  82 |     const timer = page.locator('text=/\\d+:\\d+/').first();
  83 |     const timerVisible = await timer.isVisible({ timeout: 3_000 }).catch(() => false);
  84 |     if (!timerVisible) {
  85 |       console.warn('🐛 BUG: OTP modalında geri sayım sayacı görünmüyor');
  86 |     }
  87 |   });
  88 | 
  89 | });
  90 | 
```