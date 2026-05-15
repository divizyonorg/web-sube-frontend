import { test, expect } from '@playwright/test';
import { bug } from './bug';

test.describe('Kredi Danışmanı', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediDanismani');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/KrediDanismani/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  test('form alanları görünür (Kredi Türü, Tutar, Vade)', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const fields = [
      ['Kredi Türü select', page.locator('select[name*="KrediTuru"], select[id*="KrediTuru"]').first()],
      ['Kredi Tutarı input', page.locator('input[name*="KrediTutari"], input[id*="KrediTutari"]').first()],
      ['Vade select', page.locator('select[name*="Vade"], select[id*="Vade"]').first()],
    ] as const;
    for (const [label, el] of fields) {
      if (await el.count() === 0) {
        bug(`${label} alanı bulunamadı`);
      } else {
        await expect(el).toBeVisible({ timeout: 5_000 });
      }
    }
  });

  test('"Başvuruyu Gönder" boş formda devre dışı', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const submitBtn = page.locator('button:has-text("Başvuruyu Gönder")').first();
    if (await submitBtn.count() === 0) {
      bug('"Başvuruyu Gönder" butonu sayfada bulunamadı');
      return;
    }
    if (!await submitBtn.isDisabled()) bug('Boş formda "Başvuruyu Gönder" butonu aktif — validasyon çalışmıyor');
  });

  test('form doldurulunca "Başvuruyu Gönder" aktifleşir', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const krediTuruSelect = page.locator('select[name*="KrediTuru"]').or(page.locator('select').first());
    const tutarInput     = page.locator('input[name*="KrediTutari"]').or(page.locator('input[type="number"]').first());
    const vadeSelect     = page.locator('select[name*="Vade"]').or(page.locator('select').nth(1));
    await krediTuruSelect.selectOption({ index: 1 }).catch(() => {});
    await tutarInput.fill('50000').catch(() => {});
    await vadeSelect.selectOption({ index: 1 }).catch(() => {});
    await page.waitForTimeout(300);
    const submitBtn = page.locator('button:has-text("Başvuruyu Gönder")').first();
    if (await submitBtn.count() > 0 && !await submitBtn.isEnabled())
      bug('Form doldurulunca "Başvuruyu Gönder" butonu aktifleşmiyor');
  });

  test('"Nasıl Çalışır?" bilgi bölümü görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    if (await page.locator('text=Nasıl Çalışır').first().count() === 0)
      bug('"Nasıl Çalışır?" bölümü sayfada bulunamadı');
  });

  test('iletişim bilgileri bölümü görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    if (await page.locator('text=İletişim').first().count() === 0)
      bug('İletişim bilgileri bölümü sayfada bulunamadı');
  });

});
