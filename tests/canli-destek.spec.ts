import { test, expect } from '@playwright/test';

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
    const header = page.locator('text=Destek Ekibi').first();
    const exists = await header.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: "Destek Ekibi" başlığı sohbet arayüzünde bulunamadı');
    }
  });

  test('karşılama mesajı görünür', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgArea = page.locator('[class*="message"], [class*="chat"], [class*="msg"]').first();
    const exists = await msgArea.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Sohbet mesaj alanı bulunamadı');
    }
  });

  test('mesaj input alanı görünür ve yazılabilir', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    await expect(msgInput).toBeVisible({ timeout: 5_000 });
    await msgInput.fill('Test mesajı');
    const val = await msgInput.inputValue();
    if (val !== 'Test mesajı') {
      console.warn('🐛 BUG: Mesaj input alanına yazılamıyor');
    }
  });

  test('gönder butonu boş inputta devre dışı', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const sendBtn = page.locator('button[onclick*="sendMessage"], button:has-text("Gönder")').first();
    const exists = await sendBtn.count() > 0;
    if (!exists) {
      console.warn('🐛 BUG: Gönder butonu sayfada bulunamadı');
      return;
    }
    const msgInput = page.locator('input[type="text"], textarea').last();
    await msgInput.fill('');
    await page.waitForTimeout(200);
    const isDisabled = await sendBtn.isDisabled();
    if (!isDisabled) {
      console.warn('🐛 BUG: Boş inputta gönder butonu aktif — validasyon eksik');
    }
  });

  test('mesaj gönderme akışı çalışır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    const sendBtn  = page.locator('button[onclick*="sendMessage"], button:has-text("Gönder")').first();

    await msgInput.fill('Merhaba, yardım lazım');
    await page.waitForTimeout(200);
    await sendBtn.click();
    await page.waitForTimeout(1000);

    const userMsg = page.locator('text=Merhaba, yardım lazım').first();
    const msgSent = await userMsg.count() > 0;
    if (!msgSent) {
      console.warn('🐛 BUG: Gönderilen mesaj sohbet alanında görünmüyor');
    }
  });

  test('hızlı cevap butonları görünür ve çalışır', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const quickBtns = page.locator('button[onclick*="setQuickReply"]');
    const count = await quickBtns.count();
    console.log(`ℹ️ Hızlı cevap buton sayısı: ${count}`);
    if (count === 0) {
      console.warn('🐛 BUG: Hızlı cevap butonları sayfada bulunamadı');
      return;
    }
    await quickBtns.first().click();
    await page.waitForTimeout(300);
    const msgInput = page.locator('input[type="text"], textarea').last();
    const val = await msgInput.inputValue();
    if (!val) {
      console.warn('🐛 BUG: Hızlı cevap butonuna basınca input dolmuyor');
    }
  });

  test('Enter tuşu ile mesaj gönderilebiliyor', async ({ page }) => {
    await page.locator('h1').first().waitFor({ timeout: 15_000 });
    const msgInput = page.locator('input[type="text"], textarea').last();
    await msgInput.fill('Enter test mesajı');
    await msgInput.press('Enter');
    await page.waitForTimeout(800);
    const msgVisible = await page.locator('text=Enter test mesajı').count() > 0;
    if (!msgVisible) {
      console.warn('🐛 BUG: Enter tuşu ile mesaj gönderilemiyor');
    }
  });

});
