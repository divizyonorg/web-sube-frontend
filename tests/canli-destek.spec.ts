import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

test.describe('Canlı Destek', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/CanliDestek');
    await page.waitForLoadState('domcontentloaded');
  });

  test('sayfa yüklenir ve sohbet arayüzü görünür', async ({ page }) => {
    await expect(page).toHaveURL(/CanliDestek/i);
    await expect(page.locator('h1').first()).toBeVisible({ timeout: 15_000 });
  });

  test('destek ekibi başlığı ve durum göstergesi görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    if (await page.locator('text=Destek Ekibi').first().count() === 0)
      bug('"Destek Ekibi" başlığı sohbet arayüzünde bulunamadı');
  });

  test('karşılama mesajı görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgArea = page.locator('[class*="message"], [class*="chat"], [class*="msg"]').first();
    if (await msgArea.count() === 0) bug('Sohbet mesaj alanı bulunamadı');
  });

  test('mesaj input alanı görünür ve yazılabilir', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    await expect(msgInput).toBeVisible({ timeout: 5_000 });
    await msgInput.fill('Test mesajı');
    if (await msgInput.inputValue() !== 'Test mesajı') bug('Mesaj input alanına yazılamıyor');
  });

  test('gönder butonu boş inputta devre dışı', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const sendBtn = page.locator('button[onclick*="sendMessage"], button:has-text("Gönder")').first();
    if (await sendBtn.count() === 0) {
      bug('Gönder butonu sayfada bulunamadı');
      return;
    }
    const msgInput = page.locator('input[type="text"], textarea').last();
    await msgInput.fill('');
    await page.waitForTimeout(200);
    if (!await sendBtn.isDisabled()) bug('Boş inputta gönder butonu aktif — validasyon eksik');
  });

  test('mesaj gönderme akışı çalışır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    const sendBtn  = page.locator('button[onclick*="sendMessage"], button:has-text("Gönder")').first();
    if (await sendBtn.count() === 0) {
      bug('Gönder butonu sayfada bulunamadı — mesaj gönderilemedi');
      return;
    }
    await msgInput.fill('Merhaba, yardım lazım');
    await page.waitForTimeout(200);
    await sendBtn.click();
    await page.waitForTimeout(1000);
    if (await page.locator('text=Merhaba, yardım lazım').first().count() === 0)
      bug('Gönderilen mesaj sohbet alanında görünmüyor');
  });

  test('hızlı cevap butonları görünür ve çalışır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const quickBtns = page.locator('button[onclick*="setQuickReply"]');
    info(`Hızlı cevap buton sayısı: ${await quickBtns.count()}`);
    if (await quickBtns.count() === 0) {
      bug('Hızlı cevap butonları sayfada bulunamadı');
      return;
    }
    await quickBtns.first().click();
    await page.waitForTimeout(300);
    const msgInput = page.locator('input[type="text"], textarea').last();
    if (!await msgInput.inputValue()) bug('Hızlı cevap butonuna basınca input dolmuyor');
  });

  test('Enter tuşu ile mesaj gönderilebiliyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    await msgInput.fill('Enter test mesajı');
    await msgInput.press('Enter');
    await page.waitForTimeout(800);
    if (await page.locator('text=Enter test mesajı').count() === 0)
      bug('Enter tuşu ile mesaj gönderilemiyor');
  });

});
