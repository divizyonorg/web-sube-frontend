import { test, expect } from '@playwright/test';
import { bug, info } from './bug';

// Sayısal/tutar inputlarında iki temel kural:
//   1. Değer 0 ile başlayamaz  ("0500", "0707" gibi)
//   2. String / harf girişi kabul edilmemeli
//
// Sayfalar: Finansal Profil (Ayarlar), Kredi Danışmanı, Kredi Başvurusu

// ─── Yardımcı: tek inputu üç kural için test et ──────────────────────────────
async function testSayisalInput(
  page: any,
  input: any,
  label: string,
) {
  if (await input.count() === 0) {
    info(`"${label}" input bulunamadı — alan görünmüyor veya farklı selector olabilir`);
    return;
  }

  await input.scrollIntoViewIfNeeded().catch(() => {});

  // ── Kural 1: 0 ile başlayan değer ─────────────────────────────────────────
  await input.fill('');
  await input.fill('0999');
  await input.press('Tab');
  await page.waitForTimeout(300);
  const val0 = await input.inputValue();
  info(`"${label}" — "0999" girince: "${val0}"`);
  if (val0 === '0999')
    bug(`"${label}" alanı "0999" gibi sıfırla başlayan değer kabul ediyor — başa 0 girilememelidir`);

  await input.fill('');
  await input.fill('0');
  await page.waitForTimeout(200);
  const valSingleZero = await input.inputValue();
  info(`"${label}" — tek "0" girince: "${valSingleZero}"`);
  if (valSingleZero === '0')
    bug(`"${label}" alanına tek "0" girilebiliyor — tutar 0 olamaz`);

  // ── Kural 2: String / harf girişi ─────────────────────────────────────────
  await input.fill('');
  // Playwright fill() tarayıcı validasyonunu atlar; type() ile gerçek klavye simülasyonu
  await input.type('abc');
  await page.waitForTimeout(300);
  const valStr = await input.inputValue();
  info(`"${label}" — "abc" type() girince: "${valStr}"`);
  if (/[a-zA-ZığüşöçĞÜŞÖÇİ]/.test(valStr))
    bug(`"${label}" alanına harf girilebiliyor: "${valStr}" — sadece sayı kabul edilmeli`);

  // inputmode / type attribute kontrolü
  const inputType = await input.getAttribute('type');
  const inputMode = await input.getAttribute('inputmode');
  info(`"${label}" type="${inputType}" inputmode="${inputMode}"`);
  if (inputType !== 'number' && inputMode !== 'numeric' && inputMode !== 'decimal')
    bug(`"${label}" alanında type="number" veya inputmode="numeric" yok — mobilde yazılım klavyesi sayısal açılmaz`);

  // ── Kural 3: Negatif değer ────────────────────────────────────────────────
  await input.fill('');
  await input.fill('-100');
  await input.press('Tab');
  await page.waitForTimeout(300);
  const valNeg = await input.inputValue();
  info(`"${label}" — "-100" girince: "${valNeg}"`);
  if (valNeg === '-100')
    bug(`"${label}" alanı negatif değer kabul ediyor — tutar negatif olamaz`);

  // Temizle
  await input.fill('');
}

// ─── 1. Finansal Profil: Eş'in Aylık Geliri ─────────────────────────────────
test.describe('Finansal Profil — Sayısal Input Validasyonu', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Ayarlar');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1').first().waitFor({ timeout: 15_000 });

    const tab = page.getByRole('button', { name: 'Finansal Profil' });
    if (await tab.count() > 0) await tab.click();
    await page.waitForTimeout(500);
  });

  test('[BUG-INPUT-1] Aylık net gelir — 0 ile başlayan değer ve harf girişi', async ({ page }) => {
    // Finansal profil sekmesindeki tüm sayısal inputları dene
    const candidates = [
      page.locator('input[x-model*="gelir" i]').first(),
      page.locator('input[name*="Gelir"]').first(),
      page.locator('input[placeholder*="Gelir" i]').first(),
      page.locator('input[type="number"]').first(),
    ];

    let found = false;
    for (const input of candidates) {
      if (await input.count() > 0) {
        await testSayisalInput(page, input, 'Aylık Net Gelir');
        found = true;
        break;
      }
    }
    if (!found) info('[BUG-INPUT-1] Aylık net gelir input bulunamadı — sekme içeriği kontrol edilmeli');
  });

  test('[BUG-INPUT-2] Eş\'in aylık geliri — 0 ile başlayan değer ve harf girişi', async ({ page }) => {
    // Eş çalışıyor seçeneğini aktif et
    const medeniSelect = page.locator('select[name*="MedeniHal"], select[x-model*="medeni"]').first();
    if (await medeniSelect.count() > 0) {
      await medeniSelect.selectOption({ label: 'Evli' }).catch(() => {});
      await page.waitForTimeout(400);
    }

    const esCalisiyor = page.locator('input[type="radio"][value="evet"], label:has-text("Evet") input[type="radio"]').first();
    if (await esCalisiyor.count() > 0) {
      await esCalisiyor.click({ force: true }).catch(() => {});
      await page.waitForTimeout(400);
    }

    const esGelirInput = page.locator('input[x-model="esGeliri"], input[x-model*="esGelir"]').first();
    await testSayisalInput(page, esGelirInput, 'Eş\'in Aylık Geliri');
  });

});

// ─── 2. Kredi Danışmanı: Kredi Tutarı ────────────────────────────────────────
test.describe('Kredi Danışmanı — Tutar Input Validasyonu', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediDanismani');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1, h2').first().waitFor({ timeout: 15_000 });
  });

  test('[BUG-INPUT-3] Kredi tutarı — 0 ile başlayan değer kabul edilmemeli', async ({ page }) => {
    const tutarInput = page.locator('input[x-model="tutar"], input[name*="KrediTutari"], input[placeholder*="10.000"]').first();
    await testSayisalInput(page, tutarInput, 'Kredi Tutarı');
  });

  test('[BUG-INPUT-4] Kredi tutarı — maksimum sınır attribute\'u var mı?', async ({ page }) => {
    const tutarInput = page.locator('input[x-model="tutar"], input[name*="KrediTutari"]').first();
    if (await tutarInput.count() === 0) { info('Kredi tutarı input bulunamadı'); return; }

    const maxAttr    = await tutarInput.getAttribute('max');
    const maxlenAttr = await tutarInput.getAttribute('maxlength');
    info(`Kredi tutarı max="${maxAttr}" maxlength="${maxlenAttr}"`);

    // Absürd büyük değer dene
    await tutarInput.fill('9999999999999');
    await tutarInput.press('Tab');
    await page.waitForTimeout(300);
    const val = await tutarInput.inputValue();
    info(`Kredi tutarı — "9999999999999" girince: "${val}"`);

    if (!maxAttr && !maxlenAttr)
      bug('[BUG-INPUT-4] Kredi tutarı alanında max/maxlength sınırı tanımlı değil — gerçekdışı tutar gönderilebilir');
  });

  test('[BUG-INPUT-5] Kredi tutarı — minimum sınır (negatif / sıfır) kontrolü', async ({ page }) => {
    const tutarInput = page.locator('input[x-model="tutar"], input[name*="KrediTutari"]').first();
    if (await tutarInput.count() === 0) { info('Kredi tutarı input bulunamadı'); return; }

    const minAttr = await tutarInput.getAttribute('min');
    info(`Kredi tutarı min="${minAttr}"`);

    if (!minAttr || parseInt(minAttr) <= 0)
      bug(`[BUG-INPUT-5] Kredi tutarı alanında min sınırı yok veya 0 — sıfır/negatif tutar gönderilebilir (min="${minAttr}")`);
  });

});

// ─── 3. Kredi Başvurusu Wizard: Step inputları ───────────────────────────────
test.describe('Kredi Başvurusu Wizard — Sayısal Input Validasyonu', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/KrediBasvurusu');
    await page.waitForLoadState('domcontentloaded');
    await page.waitForTimeout(1500);
  });

  test('[BUG-INPUT-6] Wizard tutar/miktar alanları — 0 ile başlayan değer ve harf girişi', async ({ page }) => {
    // Wizard'da görünür sayısal inputları bul
    const numberInputs = page.locator('input[type="number"]:visible, input[inputmode="numeric"]:visible, input[inputmode="decimal"]:visible');
    const count = await numberInputs.count();
    info(`Wizard'da görünür sayısal input sayısı: ${count}`);

    if (count === 0) {
      info('[BUG-INPUT-6] Sayısal input bulunamadı — wizard adımı henüz yüklü olmayabilir');
      return;
    }

    for (let i = 0; i < Math.min(count, 3); i++) {
      const input = numberInputs.nth(i);
      const placeholder = await input.getAttribute('placeholder') ?? `input-${i}`;
      const name        = await input.getAttribute('name')        ?? await input.getAttribute('x-model') ?? `input-${i}`;
      await testSayisalInput(page, input, `Wizard ${name || placeholder}`);
    }
  });

  test('[BUG-INPUT-7] Tüm text input alanları — sadece sayı beklenen alanlara harf girişi', async ({ page }) => {
    // Sayı beklenen text inputları: ad, soyad değil; tutar, vade, gelir gibi olanlar
    const textInputs = page.locator(
      'input[type="text"][name*="tutar" i]:visible, input[type="text"][name*="miktar" i]:visible, input[type="text"][name*="gelir" i]:visible'
    );
    const count = await textInputs.count();
    info(`Wizard'da sayı bekleyen type="text" input sayısı: ${count}`);

    for (let i = 0; i < count; i++) {
      const input = textInputs.nth(i);
      const name  = await input.getAttribute('name') ?? `text-input-${i}`;
      await input.fill('');
      await input.type('abcdef');
      await page.waitForTimeout(200);
      const val = await input.inputValue();
      info(`"${name}" — "abcdef" girince: "${val}"`);
      if (/[a-zA-ZığüşöçĞÜŞÖÇİ]/.test(val))
        bug(`[BUG-INPUT-7] "${name}" alanına harf girilebiliyor: "${val}" — type="number" veya inputmode="numeric" olmalı`);
    }
  });

});

// ─── 4. Register: Telefon alanı sadece rakam kabul etmeli ────────────────────
test.describe('Register — Telefon Input Validasyonu', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/Register');
    await page.waitForLoadState('domcontentloaded');
    await page.locator('h1, h2, [class*="form"]').first().waitFor({ timeout: 15_000 });
  });

  test('[BUG-INPUT-8] Telefon alanına harf girilememelidir', async ({ page }) => {
    const phoneInput = page.locator('input[type="tel"], input[name*="phone" i], input[name*="telefon" i], input[placeholder*="GSM"], input[placeholder*="05"]').first();
    if (await phoneInput.count() === 0) { info('Telefon input bulunamadı'); return; }

    await phoneInput.fill('');
    await phoneInput.type('abcdefg');
    await page.waitForTimeout(300);
    const val = await phoneInput.inputValue();
    info(`Telefon — "abcdefg" type() girince: "${val}"`);
    if (/[a-zA-ZığüşöçĞÜŞÖÇİ]/.test(val))
      bug(`[BUG-INPUT-8] Telefon alanına harf girilebiliyor: "${val}" — sadece rakam kabul edilmeli`);
  });

  test('[BUG-INPUT-9] Telefon alanı 10 veya 11 haneden fazla rakam kabul etmemeli', async ({ page }) => {
    const phoneInput = page.locator('input[type="tel"], input[name*="phone" i], input[name*="telefon" i], input[placeholder*="GSM"], input[placeholder*="05"]').first();
    if (await phoneInput.count() === 0) { info('Telefon input bulunamadı'); return; }

    const maxlenAttr = await phoneInput.getAttribute('maxlength');
    info(`Telefon input maxlength="${maxlenAttr}"`);

    await phoneInput.fill('05551234567890'); // 14 hane
    await page.waitForTimeout(200);
    const val = await phoneInput.inputValue().then(v => v.replace(/\D/g, ''));
    info(`Telefon — 14 rakam girince temizlenmiş değer: "${val}"`);
    if (val.length > 11)
      bug(`[BUG-INPUT-9] Telefon alanına ${val.length} hane girilebiliyor — maksimum 11 olmalı`);
  });

});
