// Global toast helper — Toastify.js üzerine ince sarıcı.
// Kullanım: toast.success('Kaydedildi'), toast.error('Hata'), toast.info('Bilgi'), toast.warning('Uyarı')
// Stillendirme ve Toastify config'i daha sonra eklenecek.
(function () {
    function show(message, variant) {
        // TODO: Toastify({ ... }).showToast(); — varyant bazlı stil eklenecek
    }

    window.toast = {
        success: function (message) { show(message, 'success'); },
        error:   function (message) { show(message, 'error'); },
        info:    function (message) { show(message, 'info'); },
        warning: function (message) { show(message, 'warning'); }
    };
})();
