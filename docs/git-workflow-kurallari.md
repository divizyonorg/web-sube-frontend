# Git Kuralları ve Workflow Dokümanı

> **Sürüm:** 1.0 | **Hedef Kitle:** Tüm Developer'lar | **Yaptırım:** Bu dokümandaki kurallar ekip genelinde zorunludur. Kurallara uyulmayan PR'lar kapatılır, kurallara uymayan commit'ler reddedilir.

---

## İçindekiler

1. [Branch'lerin Rolleri](#1-branchlerin-rolleri)
2. [Branch İsimlendirme Standartları](#2-branch-isimlendirme-standartlari)
3. [Geliştiricinin Günlük Akışı](#3-gelistiricinin-günlük-akisi)
4. [Pull Request Notları ve Kuralları](#4-pull-request-notlari-ve-kurallari)
5. [Commit Mesajı Kuralları](#5-commit-mesaji-kurallari)
6. [Merge Stratejileri](#6-merge-stratejileri)
7. [Yasaklı Anti-Pattern Davranışlar](#7-yasakli-anti-pattern-davranislar)
8. [Conflict ve Uzun Branch Engelleme Kuralları](#8-conflict-ve-uzun-branch-engelleme-kurallari)
9. [Temel Git Komutları Rehberi](#9-temel-git-komutlari-rehberi)
10. [Claude Code ile Günlük Git Akışı](#10-claude-code-ile-günlük-git-akisi)

---

## 1. Branch'lerin Rolleri

### 1.1 Kalıcı Branch'ler

Sistemde yalnızca 3 kalıcı branch bulunur. Bu branch'lere hiçbir koşulda doğrudan commit atılamaz.

| Branch | Rolü | Kimin Yönetir | Direkt Commit |
|--------|------|---------------|---------------|
| `dev` | Tüm geliştirmelerin toplandığı entegrasyon ortamı. | Chapter Lead onaylı PR ile | **YASAK** |
| `test` | Test ve doğrulama ortamı. | Chapter Lead veya yetkili kişi PR ile | **YASAK** |
| `prod` | Canlı üretim ortamı. | Chapter Lead veya yetkili kişi PR ile | **YASAK** |

**Kod akışı tek yönlüdür ve bu sıra asla tersine çevrilemez:**

```
dev  ──▶  test  ──▶  prod
```

### 1.2 Geçici Branch'ler

Geçici branch'ler, her iş başında `dev` üzerinden açılır.

| Tip | Kullanım Amacı | Örnek |
|-----|----------------|-------|
| `feature/` | Yeni bir özellik geliştirmesi | `feature/us-23` |
| `bugfix/` | Mevcut bir hatanın giderilmesi | `bugfix/us-45` |
| `refactor/` | Davranış değişmeksizin kod yapısının iyileştirilmesi | `refactor/us-67` |

> **Önemli:** Sistemde `hotfix`, `release` veya başka bir branch tipi **kesinlikle yoktur**. Canlıda çıkan bir hata dahil her türlü iş, standart `bugfix/` akışıyla ele alınır.

---

## 2. Branch İsimlendirme Standartları

### 2.1 Format

```
<tip>/us-<userStoryNumarasi>
```

**Kurallar:**

- `<tip>` yalnızca `feature`, `bugfix` veya `refactor` olabilir.
- `<userStoryNumarasi>` proje yönetim aracındaki (Jira, Linear vb.) issue/kart numarasıdır.
- Branch adında tarih, isim, soyisim veya açıklama bilgisi **yer almaz**.
- Branch adı hiçbir zaman boşluk, büyük harf veya özel karakter içermez.

### 2.2 Geçerli Örnekler

| Tip | Geçerli Branch Adı |
|-----|--------------------|
| feature | `feature/us-1234` |
| feature | `feature/us-1289` |
| bugfix | `bugfix/us-1235` |
| refactor | `refactor/us-1301` |

---

## 3. Geliştiricinin Günlük Akışı

Tüm geliştirme süreci aşağıdaki adımları takip eder. Bu adımların dışına çıkılamaz.

### Adım 1 — Issue veya User Story'yi Al

ClickApp'tan üzerine atanan issue veya user story'yi kontrol et. User story numarasını not al; branch adı ve PR başlığında kullanacaksın.

### Adım 2 — Güncel `dev`'i Yerel Makineye Çek

Branch açmadan önce her zaman `dev`'in en güncel halini al. Eski bir `dev` üzerinden açılan branch, ileride büyük conflict'lere yol açar.

```bash
git checkout dev
git pull origin dev
```

### Adım 3 — Geçici Branch Aç

`dev` güncel hale getirildikten sonra, isimlendirme standardına uygun yeni bir branch aç.

```bash
git checkout -b feature/us-1234
```

### Adım 4 — Geliştirmeyi Yap ve Commit At

Geliştirmeni yap. Commit'leri küçük, anlamlı ve commit mesajı kurallarına uygun tut. Commit'lerini düzenli aralıklarla push et.

```bash
git add .
git commit -m "feat(auth): kullanici giris formu eklendi"
git push origin feature/us-1234
```


### Adım 5 — Pull Request Aç

Geliştirme tamamlandığında PR açmadan önce güncel `dev`'i tekrar pull al ve conflict'leri kontrol et:

```bash
git fetch origin
git pull origin dev
```

Conflict varsa çöz ve commit et. Daha sonra GitHub/GitLab üzerinde `dev` branch'ini hedef alan bir PR aç. PR'ı doldurduktan sonra Slack veya uygun bir kanal üzerinden Chapter Lead'i bilgilendir. PR kuralları için bkz. [Bölüm 4](#4-pull-request-notlari-ve-kurallari).

### Adım 6 — CI ve Chapter Lead Onayı

PR açıldıktan sonra GitHub Actions CI otomatik olarak çalışır. CI başarıyla tamamlanmazsa PR merge edilemez. CI geçtikten sonra Chapter Lead PR'ı inceler, approve eder veya revizyon talep eder. CI başarıyla geçtiğinde Chapter Lead approve ederse PR merge edilir.

---

## 4. Pull Request Notları ve Kuralları

### 4.1 PR Başlığı

PR başlığı `[TIP] #UserStoryNo — Açıklama` formatında olmalıdır. Başlık, yapılan işi tek cümlede net biçimde anlatmalı; muğlak veya içeriksiz başlıklar kabul edilmez.

**Geçerli örnekler:**

```
[FEATURE] #1234 — Kullanıcı giriş formu eklendi
[BUGFIX] #1235 — Sepet sayfasındaki ürün silme hatası giderildi
```

### 4.2 PR Açıklaması

Her PR, ekibin kullandığı standart PR template'i üzerinden doldurulur. Template alanları boş bırakılamaz.

**Açıklama**

Bu PR'da ne yapıldığı ve neden yapıldığı kısaca özetlenir. "Şunu yaptım" değil, "şu bileşen, şu amaçla eklendi/değiştirildi, #UserStoryNo kapsamında tamamlandı" şeklinde ifade edilir.

**Değişiklik Türü**

PR'ın türü seçilir: `Feature`, `Bug fix` veya `Refactor`. Birden fazla tür varsa PR'ın bölünmesi gerekir.

**Test**

Manuel test yapıldığı onaylanır. "Evet çalışıyor" gibi belirsiz ifadeler kabul edilmez; ne denendiği açıklama alanına somut olarak yazılır.

**Kontrol Listesi**

PR açılmadan önce aşağıdaki maddeler eksiksiz işaretlenmiş olmalıdır:

- Kod standartlarına göre yazıldı
- Commit mesajları commit kurallarına göre uygun
- Linter hataları yok

### 4.3 Merge ve Onay Kuralı

PR'ı merge etmek için iki koşul gereklidir:

1. **GitHub Actions CI Tamamlandı** — PR'ın tüm CI kontrolleri başarıyla tamamlanmış olmalıdır. CI henüz devam ediyorsa veya başarısız olmuşsa PR merge edilemez.
2. **Chapter Lead Onayı** — CI geçtikten sonra Chapter Lead PR'ı inceleyip onay veya revizyon yorum bırakır.

**Merge Süreci**

- Developer PR'ı açtıktan sonra Slack veya uygun bir kanal üzerinden Chapter Lead'i bilgilendirir.
- Chapter Lead PR'ı inceleyip şunlardan birini yapar:
  - **Approve eder:** PR merge edilebilir hale gelir.
  - **Revizyon talep eder:** Yorum bırakır ve Developer PR'ı düzeltmesini ister.
  - **PR'ı iptal eder:** Revizyon yerine PR kapatılması gerekiyorsa iptal eder.

**PR Açılmadan Önceki Zorunlu Kontrol**

PR açılmadan önce, potansiyel conflict'leri görmek için güncel `dev` branch'ini yerel makineye pull almalısın:

```bash
git fetch origin
git pull origin dev
```

Bu adım, CI'da ve code review'da ortaya çıkabilecek conflict'leri önceden fark etmeni sağlar.

### 4.4 PR Boyutu

Bir PR mümkün olduğunca küçük tutulmalıdır. Binlerce satır değişiklik içeren PR'lar incelenmesi güç olduğundan Chapter Lead tarafından iade edilebilir. İş büyükse, mantıksal parçalara bölünerek sıralı PR'lar açılır.

---

## 5. Commit Mesajı Kuralları

### 5.1 Format

```
<tip>(<kapsam>): <açıklama>
```

- **`<tip>`** — Commit'in amacını belirler (bkz. aşağıdaki tablo).
- **`<kapsam>`** — Değişikliğin etkilediği modül veya alan. Zorunlu değil ama şiddetle tavsiye edilir.
- **`<açıklama>`** — Yapılanı özetleyen kısa, küçük harfli, Türkçe veya İngilizce (ekip standardı neyse) bir cümle. Cümle sonuna nokta konmaz.

### 5.2 Geçerli Commit Tipleri

| Tip | Ne Zaman Kullanılır |
|-----|---------------------|
| `feat` | Yeni bir özellik eklendi |
| `fix` | Bir hata düzeltildi |
| `refactor` | Davranış değişmeksizin kod yapısı değiştirildi |


### 5.3 Doğru ve Yanlış Örnekler

| Commit Mesajı | Durum | Neden |
|---------------|-------|-------|
| `feat(auth): kullanici giris formu eklendi` | ✅ Doğru | Tip, kapsam ve açıklama mevcut |
| `fix(cart): urun silme butonundaki null hatasi giderildi` | ✅ Doğru | Hatanın ne olduğu açık |
| `feat: odeme entegrasyonu` | ✅ Kabul Edilebilir | Kapsam olmadan da geçerli |
| `fix` | ❌ Hatalı | Açıklama yok, neyin fix edildiği belirsiz |
| `güncellemeler` | ❌ Hatalı | Tip yok, ne yapıldığı belirsiz |
| `WIP` | ❌ Hatalı | Anlamlı değil, PR'a dahil edilemez |
| `feat(auth): Kullanici Giris Formu Eklendi.` | ❌ Hatalı | Büyük harf ve nokta kullanılmış |
| `birden fazla şeyi tek committe yaptım` | ❌ Hatalı | Tek commit tek iş yapmalı |

### 5.4 Ek Kurallar

- Her commit **tek bir mantıksal değişikliği** kapsar. Birden fazla farklı konuyu tek commit'e sıkıştırmak yasaktır.
- `WIP` veya `temp` gibi geçici commit'ler PR'a gönderilmeden önce `git rebase -i` ile düzenlenir veya squash edilir.
- Commit mesajı 72 karakteri geçiyorsa kırpılarak bir sonraki satıra açıklama eklenir.

---

## 6. Merge Stratejileri

Ekipte tek ve zorunlu merge stratejisi **Squash and Merge**'dir. Başka hiçbir merge yöntemi kullanılmaz.

### 6.1 Squash and Merge

Geçici branch'teki tüm commit'ler tek bir commit olarak `dev`'e eklenir. Bu yöntem `dev` branch'inin geçmişini temiz, okunabilir ve her PR'ın tek bir birim olarak izlenebilir halde tutar.

Squash commit mesajı aşağıdaki formatta olmalıdır:

```
feat(auth): kullanici giris formu eklendi (#1234)
```

Parantez içindeki numara PR numarasıdır. GitHub ve GitLab, Squash and Merge seçildiğinde bu mesajı otomatik önerir; gerekiyorsa PR başlığıyla uyumlu olacak şekilde düzenlenir.

> **Not:** `rebase and merge` ve `merge commit` stratejileri ekipte kullanılmaz. Bu seçenekler platform ayarlarından devre dışı bırakılmıştır.

---

## 7. Yasaklı Anti-Pattern Davranışlar

Aşağıdaki davranışların tamamı kesinlikle yasaktır. Bu kurallara uymayan PR'lar kapatılır; tekrar eden ihlaller Chapter Lead tarafından raporlanır.

| Davranış | Neden Yasak |
|----------|-------------|
| `dev`, `test` veya `prod`'a doğrudan commit atmak | Kod inceleme ve onay sürecini tamamen devre dışı bırakır |k
| PR açmadan merge yapmak | Chapter Lead onayı alınamamış kod üretime girebilir |
| Branch'i 2 günden uzun yaşatmak | Conflict birikir, review zorlaşır, akış tıkanır |
| İsimlendirme standardına uymayan branch açmak | Kimin ne üzerinde çalıştığı izlenemez |
| Hotfix adıyla branch açmak | Sistemde hotfix süreci tanımsızdır; standart akış işletilir |
| Anlamlı olmayan commit mesajı yazmak | Geçmiş okunamaz hale gelir, debug ve audit çok zorlaşır |
| `dev` güncellemeden branch açmak | Gereksiz ve önlenebilir conflict'lere yol açar |
| Tek PR'da birden fazla user story karıştırmak | PR incelemesi anlamsızlaşır, rollback yapılamaz hale gelir |
| `push --force` kullanmak | Takım arkadaşlarının çalışmasını siler, geçmiş bozulur |

---

## 8. Conflict ve Uzun Branch Engelleme Kuralları

### 8.1 Conflict Önleme

- Branch, `dev`'in güncel hali üzerinden açılır. Açılış anında bile eski olan bir branch, zamanla büyüyen conflict riskiyle birlikte büyür.
- Geliştirme sırasında `dev`'e sık merge geliyor ve senin branch'in ile çakışma ihtimali artıyorsa, branch'ini güncel `dev` ile rebase'le:

```bash
git fetch origin
git rebase origin/dev
```

- Rebase sırasında ortaya çıkan conflict'ler dosya dosya çözülür ve `git rebase --continue` ile devam edilir.
- Rebase'den sonra uzak branch'ini güncellemek için `--force-with-lease` kullanılır (`--force` değil):

```bash
git push origin feature/us-1234 --force-with-lease
```

### 8.2 Uzun Branch Kuralı

- Bir branch açılış tarihinden itibaren **2 takvim günü** içinde PR açılmak zorundadır.
- İş 2 günde tamamlanamıyorsa iş küçük parçalara bölünür. Her parça bağımsız çalışan bir PR olarak açılır.
- Chapter Lead, açılış tarihinden 2 günden fazla geçmiş branch'leri kapatma yetkisine sahiptir.

### 8.3 Conflict Çözümünde Genel Kural

- Conflict çözümünde her zaman Chapter Lead veya ilgili kodu yazan kişiyle iletişime geç. Kendi başına "hangisi doğru?" kararını vermek yerine, kodu bilen kişiyle birlikte çöz.
- Conflict çözümü commit'i açıklayıcı bir mesajla atılır:

```bash
git commit -m "chore(merge): dev ile conflict cozuldu"
```

---

## 9. Temel Git Komutları Rehberi

Git'e henüz tam hakim olmayan developer'lar için günlük işlerde en çok kullanılan komutlar aşağıdadır.

### 9.1 Temel Komutlar

| Komut | Ne Yapar |
|-------|----------|
| `git status` | Değişen dosyaları ve branch durumunu gösterir |
| `git log --oneline` | Son commit'leri kısa formatta listeler |
| `git diff` | Henüz stage'e eklenmemiş değişiklikleri gösterir |
| `git diff --staged` | Stage'e eklenmiş ama henüz commit edilmemiş değişiklikleri gösterir |

### 9.2 Branch İşlemleri

| Komut | Ne Yapar |
|-------|----------|
| `git branch` | Yerel branch'leri listeler |
| `git branch -a` | Yerel ve uzak tüm branch'leri listeler |
| `git checkout dev` | `dev` branch'ine geçer |
| `git checkout -b feature/us-1234` | Yeni branch oluşturur ve geçer |

### 9.3 Güncelleme ve Push İşlemleri

| Komut | Ne Yapar |
|-------|----------|
| `git fetch origin` | Uzaktaki değişiklikleri indirir ama uygulamaz |
| `git pull origin dev` | `dev`'i çekip mevcut branch'e uygular |
| `git push origin <branch-adi>` | Yerel branch'i uzağa gönderir |
| `git push origin <branch-adi> --force-with-lease` | Güvenli zorla push (rebase sonrası kullanılır) |

### 9.4 Commit İşlemleri

| Komut | Ne Yapar |
|-------|----------|
| `git add .` | Tüm değişiklikleri stage'e ekler |
| `git add <dosya>` | Belirli bir dosyayı stage'e ekler |
| `git commit -m "feat(kapsam): aciklama"` | Stage'deki değişiklikleri commit eder |
| `git commit --amend` | Son commit'in mesajını veya içeriğini düzeltir (push edilmediyse) |

### 9.5 Geri Alma İşlemleri

| Komut | Ne Yapar |
|-------|----------|
| `git restore <dosya>` | Henüz stage'e eklenmemiş değişikliği geri alır |
| `git restore --staged <dosya>` | Stage'den çıkarır, değişikliği korur |
| `git revert <commit-hash>` | Commit'i geri alan yeni bir commit oluşturur (geçmişi bozmaz) |

> **Uyarı:** `git reset --hard` ve `git push --force` komutlarını çalışma branch'inde kullanmadan önce Chapter Lead'e danış.

### 9.6 Rebase İşlemleri

| Komut | Ne Yapar |
|-------|----------|
| `git rebase origin/dev` | Branch'ini güncel `dev` üzerine taşır |
| `git rebase --continue` | Conflict çözümünden sonra rebase'e devam eder |
| `git rebase --abort` | Rebase'i iptal edip eski hale döner |
| `git rebase -i HEAD~3` | Son 3 commit'i interaktif olarak düzenler (squash, mesaj değiştirme vb.) |

---

## 10. Claude Code ile Günlük Git Akışı

Bu bölüm, Claude Code terminal üzerinden çalışan developer'lar için günlük iş başlangıcından PR açmaya kadar adım adım Git akışını açıklar.

### 10.1 Güne Başlama — Dev'i Güncelle

Her güne ve her yeni işe başlamadan önce `dev`'in güncel halini çek.

```bash
git checkout dev
git pull origin dev
```

Bu adımı atlamak, başkasının merge ettiği kodlarla conflict yaşamana neden olur.

### 10.2 Yeni Branch Aç

Güncel `dev` üzerinden, isimlendirme standardına uygun branch'ini oluştur.

```bash
git checkout -b feature/us-1289
```

### 10.3 Claude Code ile Geliştir

Claude Code terminalinde çalışmaya başla. AI'dan kod üretmesini veya değişiklik yapmasını iste. Her mantıklı iş birimi tamamlandığında commit at; tüm geliştirmeyi bitirip tek seferde commit atmak yasaktır.

```bash
# Değişiklikleri kontrol et
git status
git diff

# Stage'e ekle
git add .

# Commit at
git commit -m "feat(profile): kullanici avatar yukleme eklendi"
```

Claude Code'a yazdırılan kodlar için commit mesajını sen yazarsın; AI'ın commit geçmişinde izlenebilirliği sen sağlarsın.

### 10.4 Commit'leri Uzağa Gönder

Düzenli aralıklarla commit'lerini push et. Böylece hem çalışman kaybolmaz hem de Chapter Lead güncel durumu takip edebilir.

```bash
git push origin feature/us-1289
```

### 10.5 PR Öncesi Kontrol

PR açmadan önce aşağıdaki kontrolleri yap:

```bash
# 1. dev'in güncel olup olmadığını kontrol et ve gerekirse rebase yap
git fetch origin
git rebase origin/dev

# 2. Commit geçmişini gözden geçir, WIP veya temp commit varsa düzelt
git log --oneline

# 3. Kalan değişiklik olmadığından emin ol
git status

# 4. Son push
git push origin feature/us-1289
```

### 10.6 PR Aç

GitHub veya GitLab arayüzünde `dev` branch'ini hedef alan PR'ı aç:

- **Başlık formatı:** `[FEATURE] #1289 — Kullanici profil avatar yukleme`
- **Açıklamayı** standart PR template üzerinden eksiksiz doldur.
- **Assignee** olarak Chapter Lead'i ata.
- Slack veya ekip kanalından Chapter Lead'i PR'dan haberdar et.

### 10.7 Onay ve Merge Sonrası

Chapter Lead PR'ı onaylayıp merge ettikten sonra `dev`'e geç ve güncelle.

```bash
git checkout dev
git pull origin dev
```

Yeni bir issue aldıysan [Adım 10.2](#102-yeni-branch-aç)'den tekrar başla.

---

*Bu doküman yaşayan bir belgedir. Kural değişikliği veya güncelleme için Chapter Lead ile iletişime geç.*
