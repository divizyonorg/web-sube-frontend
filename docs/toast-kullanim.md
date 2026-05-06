# Toast Bildirimleri — Kullanım Kılavuzu

`wwwroot/js/toast.js` dosyası `_Layout.cshtml` üzerinden **tüm sayfalara otomatik yüklenir**.
Kurulum veya import gerekmez. Herhangi bir `.cshtml` sayfasında ya da `.js` dosyasında doğrudan çağrılır.

---

## Kısa Kullanım (Önerilen)

```js
toast.success("İşlem başarıyla tamamlandı.");
toast.error("Bir hata oluştu, lütfen tekrar deneyin.");
toast.info("Dosyanız işleniyor...");
toast.warning("Oturum süreniz dolmak üzere.");
```

İkinci parametre ile ek seçenekler geçilebilir:

```js
toast.success("Profil güncellendi.", { duration: 6000 });
toast.error("Bağlantı hatası.",      { closable: false });
toast.info("Yükleniyor...",          { position: "top-right", pauseOnHover: false });
```

---

## Tam API — `Toast.show(options)`

Tüm seçeneklere ihtiyaç duyulduğunda `Toast.show()` kullanılır.

```js
Toast.show({
    variant     : "success",       // "success" | "error" | "info" | "warning"
    message     : "Ana mesaj",     // zorunlu
    description : "Alt açıklama", // opsiyonel — mesajın altında küçük yazı çıkar
    closable    : true,            // sağ üstte ✕ butonu (varsayılan: true)
    duration    : 4000,            // otomatik kapanma ms (varsayılan: 4000)
    position    : "bottom-right",  // konum (varsayılan: "bottom-right")
    pauseOnHover: true,            // üzerine gelinince süre durur (varsayılan: true)
});
```

### Konum Seçenekleri

| `position` değeri | Açıklama |
|---|---|
| `"bottom-right"` | Sağ alt — varsayılan |
| `"bottom-center"` | Alt orta |
| `"top-right"` | Sağ üst |
| `"top-center"` | Üst orta |

---

## Razor Pages `.cshtml` İçinde Kullanım

### Buton tıklamasında

```html
<button type="button" onclick="toast.success('Kaydedildi.')">
    Kaydet
</button>
```

### fetch / AJAX sonrasında

```js
fetch("/api/profile", { method: "POST", body: formData })
    .then(res => {
        if (res.ok) toast.success("Profil güncellendi.");
        else        toast.error("Güncelleme başarısız.");
    })
    .catch(() => toast.error("Sunucuya bağlanılamadı."));
```

### HTMX olaylarında

```html
<form hx-post="/Apply/Submit"
      hx-on::after-request="
          event.detail.successful
              ? toast.success('Başvurunuz alındı.')
              : toast.error('Başvuru gönderilemedi.')
      ">
</form>
```

### JustValidate ile birlikte

```js
validation.onFail(()    => toast.warning("Lütfen eksik alanları doldurun."));
validation.onSuccess(() => toast.info("Form gönderiliyor..."));
```

### Sunucu tarafından mesaj (TempData köprüsü)

C# tarafından doğrudan toast tetiklenemez. Sunucu mesajını sayfaya TempData ile taşıyıp,
`@section Scripts` içinde JS ile çağırın:

```html
@if (TempData["SuccessMessage"] is string msg)
{
    <script>toast.success('@msg');</script>
}
```

---

## Alt Açıklamalı Toast

```js
Toast.show({
    variant    : "error",
    message    : "Yükleme başarısız",
    description: "Dosya boyutu 10 MB sınırını aşıyor.",
    duration   : 6000,
});
```

---

## Davranış Notları

| Kural | Detay |
|---|---|
| Maksimum 4 toast | 5. geldiğinde en eskisi otomatik kalkar |
| `pauseOnHover: true` | Fare üzerindeyken geri sayım durur, çekilince devam eder |
| `duration: 0` | Toast hiç kapanmaz, yalnızca ✕ ile kapatılabilir |
| Animasyon | Giriş ve çıkış slide + fade ile yapılır |
