import { expect, test } from '@playwright/test';

export function bug(id: string | number, title?: string): void {
  const label = title ? `[${id}] ${title}` : String(id);
  try {
    test.info().annotations.push({ type: 'bug', description: label });
  } catch {
    // test.info() context dışında çağrılırsa sessizce geç
  }
  console.warn(`🐛 BUG: ${label}`);
  expect.soft(false, `🐛 BUG: ${label}`).toBe(true);
}

export function info(message: string): void {
  try {
    test.info().annotations.push({ type: 'info', description: message });
  } catch {
    // test.info() context dışında çağrılırsa sessizce geç
  }
  console.log(`ℹ️ ${message}`);
}
