// Uygulama giriş noktası — formlar ve kütüphaneler burada bağlanır.

document.addEventListener('alpine:init', () => {
    Alpine.store('layout', {
        mobileMenuOpen: false
    });
});

