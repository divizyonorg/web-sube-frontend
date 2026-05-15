import { test, expect } from '@playwright/test';
import { bug } from './bug';

test.describe('Sözleşmelerim', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Sozlesmelerim');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir', async ({ page }) => {
    await expect(page).toHaveURL(/Sozlesmelerim/i);
  });

  test('"YAKINDA" modalı otomatik açılır', async ({ page }) => {
    const modal = page.locator('text=YAKINDA').first();
    await expect(modal).toBeVisible({ timeout: 10_000 });
  });

  test('"YAKINDA" modal mesajı doğru', async ({ page }) => {
    await page.locator('text=YAKINDA').first().waitFor({ timeout: 10_000 });
    const msg = page.locator('text=yakında').or(page.locator('text=hizmetinize açılacak')).first();
    if (await msg.count() === 0) bug('YAKINDA modal mesajı beklendiği gibi değil');
  });

});
