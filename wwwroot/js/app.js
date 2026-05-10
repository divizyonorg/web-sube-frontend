// Uygulama giriş noktası — formlar ve kütüphaneler burada bağlanır.

function logout() {
    localStorage.removeItem('auth_token');
    window.location.href = '/login';
}
