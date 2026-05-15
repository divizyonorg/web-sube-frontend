# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: login.spec.ts >> Login Sayfası >> GSM input alanı görünür ve IMask maskeleme çalışır
- Location: tests\login.spec.ts:17:7

# Error details

```
Error: 🐛 BUG: IMask telefon maskeleme beklenmedik format üretti: +95 389 503 49 6

expect(received).toBe(expected) // Object.is equality

Expected: true
Received: false
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
        - textbox "GSM" [active] [ref=e14]: +95 389 503 49 6
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
  1  | import { expect } from '@playwright/test';
  2  | 
  3  | export function bug(message: string): void {
  4  |   console.warn(`🐛 BUG: ${message}`);
> 5  |   expect.soft(false, `🐛 BUG: ${message}`).toBe(true);
     |                                            ^ Error: 🐛 BUG: IMask telefon maskeleme beklenmedik format üretti: +95 389 503 49 6
  6  | }
  7  | 
  8  | export function info(message: string): void {
  9  |   console.log(`ℹ️ ${message}`);
  10 | }
  11 | 
```