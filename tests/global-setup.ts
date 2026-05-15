import { chromium } from '@playwright/test';
import * as fs from 'fs';
import * as path from 'path';

const BASE_URL = process.env.BASE_URL ?? 'https://websube.divizyon.org';
const GSM      = process.env.TEST_GSM  ?? '';
const TCKN     = process.env.TEST_TCKN ?? '';

export default async function globalSetup() {
  if (!GSM || !TCKN) {
    throw new Error(
      'TEST_GSM ve TEST_TCKN ortam değişkenleri tanımlanmamış.\n' +
      'Örnek: TEST_GSM="+90 5XX XXX XX XX" TEST_TCKN="XXXXXXXXXXX" npx playwright test'
    );
  }

  const authDir  = path.join('tests', '.auth');
  const authFile = path.join(authDir, 'state.json');
  if (!fs.existsSync(authDir)) fs.mkdirSync(authDir, { recursive: true });

  const browser = await chromium.launch({ headless: false, slowMo: 300 });
  const context = await browser.newContext();
  const page    = await context.newPage();

  await page.goto(BASE_URL + '/login');

  // Adım 1 — GSM
  await page.getByRole('textbox', { name: 'GSM' }).fill(GSM);
  await page.getByRole('button', { name: 'Devam Et' }).click();

  // Adım 2 — TCKN
  await page.getByRole('textbox', { name: 'TCKN' }).fill(TCKN);
  await page.getByRole('button', { name: 'Giriş Yap', exact: true }).click();

  // Adım 3 — OTP: terminale yaz, kullanıcı tarayıcıya girer
  console.log('\n📱 SMS kodu telefonuna geldi. Tarayıcıya kodu girip "Devam Et"e tıkla.');
  console.log('⏳ 2 dakika bekleniyor...\n');

  await page.waitForURL('**/anasayfa', { timeout: 120_000 });

  await context.storageState({ path: authFile });
  await browser.close();

  console.log('✅ Oturum kaydedildi → tests/.auth/state.json\n');
}
