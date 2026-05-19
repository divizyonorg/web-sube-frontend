// Uygulama giriş noktası — formlar ve kütüphaneler burada bağlanır.

document.addEventListener('alpine:init', () => {
    Alpine.store('layout', { mobileMenuOpen: false });
    Alpine.store('krediModal', { open: false, confirmClose: false });
});

// ── Kredi modal global yardımcıları ──────────────────────────────────────────
// Alpine store + doğrudan DOM — ikisi birlikte, timing bağımsız

window.openKrediModal = function () {
    if (window.Alpine) Alpine.store('krediModal').open = true;
    var el = document.getElementById('kredi-modal-overlay');
    if (el) el.style.display = 'flex';
};

window.showKrediConfirm = function () {
    if (window.Alpine) Alpine.store('krediModal').confirmClose = true;
    var el = document.getElementById('kredi-confirm-overlay');
    if (el) el.style.display = 'flex';
};

window.hideKrediConfirm = function () {
    if (window.Alpine) Alpine.store('krediModal').confirmClose = false;
    var el = document.getElementById('kredi-confirm-overlay');
    if (el) el.style.display = 'none';
};

window.closeKrediModal = function () {
    window.hideKrediConfirm();
    if (window.Alpine) Alpine.store('krediModal').open = false;
    var overlay = document.getElementById('kredi-modal-overlay');
    if (overlay) overlay.style.display = 'none';
    setTimeout(function () {
        if (window.htmx) htmx.ajax('GET', '/KrediBasvurusu?handler=Step1', { target: '#wizard-container', swap: 'innerHTML' });
    }, 300);
};

// Wizard içindeki "/anasayfa" close butonlarını yakala → onay diyaloğu aç
document.addEventListener('click', function (e) {
    var link = e.target.closest('a[href="/anasayfa"]');
    if (!link) return;
    var container = document.getElementById('wizard-container');
    if (!container || !container.contains(link)) return;
    e.preventDefault();
    window.showKrediConfirm();
});

// HTMX swap sonrası wizard içindeki Alpine bileşenlerini başlat
document.body.addEventListener('htmx:afterSwap', function (e) {
    if (e.detail.target.id === 'wizard-container' && window.Alpine) Alpine.initTree(e.detail.target);
});
