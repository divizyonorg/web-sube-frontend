# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: kredi-basvurusu.spec.ts >> Kredi Başvurusu >> devam butonu görünür
- Location: tests\kredi-basvurusu.spec.ts:29:7

# Error details

```
Error: 🐛 BUG: Kredi başvurusu devam/ileri butonu bulunamadı

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
          - link "Geri" [ref=e65] [cursor=pointer]:
            - /url: /anasayfa
            - generic [ref=e66]: Geri
          - generic [ref=e67]:
            - generic [ref=e68]: İnteraktif Kredi
            - generic [ref=e69]: WEB Şube
        - generic [ref=e71]:
          - link [ref=e72] [cursor=pointer]:
            - /url: /anasayfa
          - img "İnteraktif Kredi" [ref=e74]
          - generic [ref=e75]:
            - heading "Senin için en doğru finansal rotayı çizelim" [level=2] [ref=e76]
            - paragraph [ref=e77]: Birkaç kısa soruyla ihtiyacını anlayalım
          - generic [ref=e78]:
            - button "Hemen krediye ihtiyacım var" [ref=e79] [cursor=pointer]:
              - generic [ref=e81]: Hemen krediye ihtiyacım var
            - button "Sadece kredi profilimi öğrenmek istiyorum" [ref=e82] [cursor=pointer]:
              - generic [ref=e84]: Sadece kredi profilimi öğrenmek istiyorum
          - paragraph [ref=e86]:
            - text: Bu bilgiler kredi notunu etkilemez.
            - text: Sadece sana özel öneriler sunmamıza yardımcı olur.
      - generic [ref=e87]:
        - generic [ref=e88]:
          - generic [ref=e89]: 1M+ kullanıcı bize güveniyor.
          - generic [ref=e90]: İnteraktif Kredi A.Ş. © 2026
        - paragraph [ref=e92]: "© 2024 İnteraktif Kredi A.Ş. Her hakkı saklıdır. İnteraktif Kredi'nin resmi ünvanı İnteraktifKredi Danışmanlık AŞ'dir. Bu site yalnızca Türkiye'deki kişilere yöneliktir. İstanbul Ticaret Odası ticaret sicil no: 85502 – MERSİS NO: 0478056624800001"
```

# Test source

```ts
  1  | import { expect } from '@playwright/test';
  2  | 
  3  | export function bug(message: string): void {
  4  |   console.warn(`🐛 BUG: ${message}`);
> 5  |   expect.soft(false, `🐛 BUG: ${message}`).toBe(true);
     |                                            ^ Error: 🐛 BUG: Kredi başvurusu devam/ileri butonu bulunamadı
  6  | }
  7  | 
  8  | export function info(message: string): void {
  9  |   console.log(`ℹ️ ${message}`);
  10 | }
  11 | 
```