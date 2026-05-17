/// <reference types="node" />

import { defineConfig, devices } from '@playwright/test';

const BASE_URL = process.env.BASE_URL ?? 'https://websube.divizyon.org';

export default defineConfig({
  testDir: './tests',
  globalSetup: './tests/global-setup.ts',

  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,

  reporter: [
    ['list'],
    ['html', { outputFolder: 'playwright-report', open: 'never' }],
  ],

  use: {
    baseURL: BASE_URL,
    trace: 'on',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    launchOptions: {
      slowMo: process.env.SLOW_MO ? parseInt(process.env.SLOW_MO) : 0,
    },
  },

  projects: [
    // Oturum gerektirmeyen testler — storageState yok
    {
      name: 'chromium-unauth',
      use: { ...devices['Desktop Chrome'] },
      testMatch: [
        'tests/login.spec.ts',
        'tests/register.spec.ts',
      ],
    },
    // Oturum gerektiren testler — globalSetup'ın ürettiği state kullanılır
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        storageState: 'tests/.auth/state.json',
      },
      testMatch: [
        'tests/anasayfa.spec.ts',
        'tests/navigasyon.spec.ts',
        'tests/kredi-basvurusu.spec.ts',
        'tests/kredi-raporlari.spec.ts',
        'tests/sana-ozel-teklifler.spec.ts',
        'tests/vip-paketler.spec.ts',
        'tests/kredi-danismani.spec.ts',
        'tests/destek-merkezi.spec.ts',
        'tests/canli-destek.spec.ts',
        'tests/faturalarim.spec.ts',
        'tests/sozlesmelerim.spec.ts',
        'tests/ayarlar.spec.ts',
        'tests/finansal-profil.spec.ts',
        'tests/odeme.spec.ts',
        'tests/input-validasyon.spec.ts',
        'tests/clients.spec.ts',
      ],
    },
  ],
});
