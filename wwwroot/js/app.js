// Uygulama giriş noktası — formlar ve kütüphaneler burada bağlanır.

document.addEventListener('alpine:init', () => {
    Alpine.store('layout', { mobileMenuOpen: false });
    Alpine.store('krediModal', { open: false, confirmClose: false });
});

// Wizard içindeki "/anasayfa" close butonlarını yakala → modal kapat + Step1'e sıfırla
document.addEventListener('click', function (e) {
    var link = e.target.closest('a[href="/anasayfa"]');
    if (!link) return;
    var container = document.getElementById('wizard-container');
    if (!container || !container.contains(link)) return;

    e.preventDefault();
    if (window.Alpine && Alpine.store) Alpine.store('krediModal').confirmClose = true;
});

// HTMX swap sonrası wizard içindeki Alpine bileşenlerini başlat
document.body.addEventListener('htmx:afterSwap', function (e) {
    if (e.detail.target.id === 'wizard-container' && window.Alpine) Alpine.initTree(e.detail.target);
});

