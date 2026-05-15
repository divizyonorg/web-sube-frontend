# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: canli-destek.spec.ts >> Canlı Destek >> karşılama mesajı görünür
- Location: tests\canli-destek.spec.ts:22:7

# Error details

```
Error: 🐛 BUG: Sohbet mesaj alanı bulunamadı

expect(received).toBe(expected) // Object.is equality

Expected: true
Received: false
```

# Page snapshot

```yaml
- generic [active] [ref=e1]:
  - banner [ref=e3]:
    - generic [ref=e4]:
      - img "İnteraktif Kredi" [ref=e6]
      - generic [ref=e8]:
        - button [ref=e9] [cursor=pointer]
        - button "Profil fotoğrafı HATİCE KEMENÇE" [ref=e10] [cursor=pointer]:
          - img "Profil fotoğrafı" [ref=e11]
          - generic [ref=e12]: HATİCE KEMENÇE
  - generic [ref=e13]:
    - generic [ref=e16]:
      - button [ref=e18] [cursor=pointer]
      - generic [ref=e20]:
        - img [ref=e21]
        - textbox "Ara..." [ref=e24]
      - navigation [ref=e25]:
        - link "Anasayfa" [ref=e26] [cursor=pointer]:
          - /url: /anasayfa
          - generic [ref=e27]: Anasayfa
        - link "Kredi Başvurusu" [ref=e28] [cursor=pointer]:
          - /url: /KrediBasvurusu
          - generic [ref=e29]: Kredi Başvurusu
        - link "Kredi Raporların" [ref=e30] [cursor=pointer]:
          - /url: /KrediRaporlari
          - generic [ref=e31]: Kredi Raporların
        - link "Sana Özel Teklifler" [ref=e32] [cursor=pointer]:
          - /url: /SanaOzelTeklifler
          - generic [ref=e33]: Sana Özel Teklifler
        - link "Kredi Danışmanı" [ref=e34] [cursor=pointer]:
          - /url: /KrediDanismani
          - generic [ref=e35]: Kredi Danışmanı
        - link "Destek Merkezi" [ref=e36] [cursor=pointer]:
          - /url: /DestekMerkezi
          - generic [ref=e37]: Destek Merkezi
        - link "Canlı Destek" [ref=e38] [cursor=pointer]:
          - /url: /CanliDestek
          - generic [ref=e39]: Canlı Destek
        - link "Faturaların" [ref=e40] [cursor=pointer]:
          - /url: /Faturalarin
          - generic [ref=e41]: Faturaların
        - link "Sözleşmelerin" [ref=e42] [cursor=pointer]:
          - /url: /Sozlesmelerim
          - generic [ref=e43]: Sözleşmelerin
        - link "Ayarlar" [ref=e44] [cursor=pointer]:
          - /url: /Ayarlar
          - generic [ref=e45]: Ayarlar
      - generic [ref=e47]:
        - generic [ref=e48]:
          - img [ref=e49]
          - generic [ref=e51]: VIP Danışmanlık
        - paragraph [ref=e52]: Krediye her zaman hazır olun. Ayrıcalıklı hizmet.
        - link "Paketi İncele" [ref=e53] [cursor=pointer]:
          - /url: /VipDanismalikPaketleri
          - generic [ref=e54]: Paketi İncele
        - img [ref=e55]
      - generic [ref=e58]:
        - button [ref=e59] [cursor=pointer]
        - button [ref=e60] [cursor=pointer]
    - main [ref=e61]:
      - generic [ref=e63]:
        - generic [ref=e64]:
          - heading "Canlı Destek" [level=1] [ref=e65]
          - paragraph [ref=e66]: Anlık destek için bizimle sohbet edin
        - generic [ref=e67]:
          - generic [ref=e69]:
            - img [ref=e71]
            - generic [ref=e75]:
              - generic [ref=e76]: Destek Ekibi
              - generic [ref=e77]: Aktif
          - generic [ref=e79]:
            - img [ref=e81]
            - generic [ref=e85]:
              - paragraph [ref=e87]: Merhaba! Hallederiz destek ekibine hoş geldiniz. Size nasıl yardımcı olabilirim?
              - generic [ref=e88]: 23:06
          - generic [ref=e90]:
            - button "Kredi başvurusu yapmak istiyorum" [ref=e91] [cursor=pointer]
            - button "Ödeme planımı görmek istiyorum" [ref=e92] [cursor=pointer]
            - button "Hesap bilgilerimi güncellemek istiyorum" [ref=e93] [cursor=pointer]
          - generic [ref=e95]:
            - textbox "Mesajınızı yazın..." [ref=e96]
            - button [disabled] [ref=e97]:
              - img [ref=e98]
      - generic [ref=e101]:
        - generic [ref=e102]:
          - generic [ref=e103]: 1M+ kullanıcı bize güveniyor.
          - generic [ref=e104]: İnteraktif Kredi A.Ş. © 2026
        - paragraph [ref=e106]: "© 2024 İnteraktif Kredi A.Ş. Her hakkı saklıdır. İnteraktif Kredi'nin resmi ünvanı İnteraktifKredi Danışmanlık AŞ'dir. Bu site yalnızca Türkiye'deki kişilere yöneliktir. İstanbul Ticaret Odası ticaret sicil no: 85502 – MERSİS NO: 0478056624800001"
```

# Test source

```ts
  1  | import { expect } from '@playwright/test';
  2  | 
  3  | export function bug(message: string): void {
  4  |   console.warn(`🐛 BUG: ${message}`);
> 5  |   expect.soft(false, `🐛 BUG: ${message}`).toBe(true);
     |                                            ^ Error: 🐛 BUG: Sohbet mesaj alanı bulunamadı
  6  | }
  7  | 
  8  | export function info(message: string): void {
  9  |   console.log(`ℹ️ ${message}`);
  10 | }
  11 | 
```