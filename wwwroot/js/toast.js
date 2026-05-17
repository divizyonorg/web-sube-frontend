const Toast = (() => {
    const MAX = 4;
    const _containers = {};
    const _activeMessages = new Set();

    const _borderColor = {
        success: '#36FC99',
        error:   '#DC2626',
        info:    '#2E6DF8',
        warning: '#F59E0B',
    };

    const _icon = {
        success: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#36FC99" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10"/><polyline points="9 12 11 14 15 10"/></svg>`,
        error:   `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#DC2626" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10"/><line x1="15" y1="9" x2="9" y2="15"/><line x1="9" y1="9" x2="15" y2="15"/></svg>`,
        info:    `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#2E6DF8" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10"/><line x1="12" y1="16" x2="12" y2="12"/><line x1="12" y1="8" x2="12.01" y2="8"/></svg>`,
        warning: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#F59E0B" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/><line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>`,
    };

    const _closeIcon = `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#9CA3AF" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>`;

    function _getContainer(position) {
        if (_containers[position]) return _containers[position];

        const isBottom = position.startsWith('bottom');
        const isCenter = position.endsWith('center');

        const el = document.createElement('div');
        Object.assign(el.style, {
            position: 'fixed',
            zIndex: '9999',
            display: 'flex',
            // column-reverse: prepend her zaman ekrana en yakın köşede çıkar
            flexDirection: isBottom ? 'column-reverse' : 'column',
            gap: '8px',
            pointerEvents: 'none',
        });

        el.style[isBottom ? 'bottom' : 'top'] = '16px';

        if (isCenter) {
            el.style.left = '50%';
            el.style.transform = 'translateX(-50%)';
        } else {
            el.style.right = '16px';
        }

        document.body.appendChild(el);
        _containers[position] = el;
        return el;
    }

    function _dismiss(toast, isBottom) {
        toast.style.transition = 'opacity 200ms ease-in, transform 200ms ease-in';
        toast.style.opacity = '0';
        toast.style.transform = `translateY(${isBottom ? '16px' : '-16px'})`;
        setTimeout(() => {
            _activeMessages.delete(toast.dataset.toastKey || '');
            toast.remove();
        }, 200);
    }

    function show({
        variant = 'info',
        message = '',
        description = '',
        closable = true,
        duration = 4000,
        position = 'bottom-right',
        pauseOnHover = true,
    } = {}) {
        // Aynı mesaj zaten gösteriliyorsa tekrar render etme
        const dedupeKey = variant + ':' + message;
        if (_activeMessages.has(dedupeKey)) return;
        _activeMessages.add(dedupeKey);

        const container = _getContainer(position);
        const isBottom = position.startsWith('bottom');

        // Max 4 — en eskiyi (son child) kaldır
        const existing = container.querySelectorAll('[data-toast]');
        if (existing.length >= MAX) {
            _dismiss(existing[existing.length - 1], isBottom);
        }

        const toast = document.createElement('div');
        toast.dataset.toast = '';
        toast.dataset.toastKey = dedupeKey;
        toast.setAttribute('role', 'status');
        toast.setAttribute('aria-live', 'polite');

        Object.assign(toast.style, {
            background: '#FFFFFF',
            borderLeft: `4px solid ${_borderColor[variant] ?? _borderColor.info}`,
            borderRadius: '8px',
            boxShadow: '0 8px 24px rgba(0,63,117,0.12)',
            padding: '14px 16px',
            minWidth: '300px',
            maxWidth: '480px',
            display: 'flex',
            alignItems: 'flex-start',
            gap: '12px',
            opacity: '0',
            transform: `translateY(${isBottom ? '16px' : '-16px'})`,
            transition: 'opacity 300ms ease-out, transform 300ms ease-out',
            pointerEvents: 'all',
            boxSizing: 'border-box',
        });

        const descHtml = description
            ? `<div style="font-size:13px;color:#6B7280;margin-top:2px;">${description}</div>`
            : '';

        const closeHtml = closable
            ? `<button data-toast-close style="background:none;border:none;cursor:pointer;padding:0;flex-shrink:0;display:flex;align-items:center;margin-left:auto;" aria-label="Kapat">${_closeIcon}</button>`
            : '';

        toast.innerHTML = `
            <div style="flex-shrink:0;margin-top:1px;">${_icon[variant] ?? _icon.info}</div>
            <div style="flex:1;min-width:0;">
                <div style="font-size:14px;font-weight:500;color:#111827;">${message}</div>
                ${descHtml}
            </div>
            ${closeHtml}
        `;

        // Prepend — her zaman ekrana en yakın kenarda görünür
        container.insertBefore(toast, container.firstChild);

        // Giriş animasyonu
        requestAnimationFrame(() => requestAnimationFrame(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateY(0)';
        }));

        // Otomatik kapanma zamanlayıcısı
        let remaining = duration;
        let startTime = Date.now();
        let timer = setTimeout(() => _dismiss(toast, isBottom), remaining);

        if (pauseOnHover) {
            toast.addEventListener('mouseenter', () => {
                clearTimeout(timer);
                remaining -= Date.now() - startTime;
            });
            toast.addEventListener('mouseleave', () => {
                startTime = Date.now();
                timer = setTimeout(() => _dismiss(toast, isBottom), remaining);
            });
        }

        if (closable) {
            toast.querySelector('[data-toast-close]')?.addEventListener('click', () => {
                clearTimeout(timer);
                _activeMessages.delete(dedupeKey);
                _dismiss(toast, isBottom);
            });
        }
    }

    return { show };
})();

// Kısa yol yardımcıları — mevcut kullanımlarla uyumlu
window.Toast = Toast;
window.toast = {
    success: (message, opts) => Toast.show({ variant: 'success', message, ...opts }),
    error:   (message, opts) => Toast.show({ variant: 'error',   message, ...opts }),
    info:    (message, opts) => Toast.show({ variant: 'info',    message, ...opts }),
    warning: (message, opts) => Toast.show({ variant: 'warning', message, ...opts }),
};
