import { expect } from '@playwright/test';

export function bug(message: string): void {
  console.warn(`🐛 BUG: ${message}`);
  expect.soft(false, `🐛 BUG: ${message}`).toBe(true);
}

export function info(message: string): void {
  console.log(`ℹ️ ${message}`);
}
