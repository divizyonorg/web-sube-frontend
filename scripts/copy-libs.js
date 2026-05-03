const fs   = require('fs');
const path = require('path');
const vm   = require('vm');

function copy(src, dest) {
    const destDir = path.dirname(dest);
    fs.mkdirSync(destDir, { recursive: true });
    if (fs.existsSync(src)) {
        fs.copyFileSync(src, dest);
        console.log(`  ✓ ${path.relative('wwwroot/lib', dest)}`);
    } else {
        console.warn(`  ✗ Bulunamadı: ${src}`);
    }
}

// kebab-case → PascalCase  (örn. "layout-alt-02" → "LayoutAlt02")
function kebabToPascal(name) {
    return name.split('-').map(s => s[0].toUpperCase() + s.slice(1)).join('');
}

// SVG'de camelCase kalan atribute'lar (kebab'a dönüşmemeli)
const SVG_CAMEL_ATTRS = new Set(['viewBox', 'gradientUnits', 'gradientTransform', 'patternUnits', 'clipPath', 'clipPathUnits']);

// camelCase → kebab-case  (örn. "strokeWidth" → "stroke-width")
// SVG'ye özgü camelCase atribute'lar korunur
function camelToKebab(str) {
    if (SVG_CAMEL_ATTRS.has(str)) return str;
    return str.replace(/([A-Z])/g, m => '-' + m.toLowerCase());
}

// React createElement ağacını SVG string'e dönüştür
function renderNode(node) {
    if (!node || typeof node === 'string') return node || '';

    const { tag, props = {}, children = [] } = node;
    const skip = new Set(['width', 'height', 'aria-hidden', 'ariahidden', 'ariaHidden']);

    const attrs = Object.entries(props)
        .filter(([k, v]) => !skip.has(k) && !skip.has(camelToKebab(k)) && v != null && typeof v !== 'function')
        .map(([k, v]) => `${camelToKebab(k)}="${v}"`)
        .join(' ');

    const childStr = children.map(renderNode).join('');

    if (tag === 'svg') {
        return `<svg xmlns="http://www.w3.org/2000/svg" ${attrs}>${childStr}</svg>`;
    }

    const el = attrs ? `${tag} ${attrs}` : tag;
    return childStr ? `<${el}>${childStr}</${tag}>` : `<${el}/>`;
}

// @untitledui/icons .mjs dosyasından SVG üret — React mock'lanır, gerçek render yok
function extractSvgFromMjs(mjsPath, color) {
    const content = fs.readFileSync(mjsPath, 'utf8');

    const mock = {
        createElement(tag, props, ...children) {
            return { tag, props: props || {}, children: children.flat().filter(c => c != null) };
        }
    };

    // Her dosyada React import alias farklı olabilir (a, r, o, e…) — doğru alias'ı oku
    const aliasMatch = content.match(/import\*as (\w+) from"react";/);
    const reactAlias = aliasMatch ? aliasMatch[1] : 'a';

    // Kodu vm için hazırla: import kaldır, export kaldır, displayName kaldır,
    // component fonksiyonunu context'te erişilebilir yap
    const code = content
        .replace(/import\*as \w+ from"react";/, '')
        .replace(/\w+\.displayName=[^;]+;/, '')
        .replace(/export\{[^}]+\};?\s*$/, '')
        .replace(/const (\w+)=(\([^)]*\)=>)/, '__comp=$2');

    const ctx = { [reactAlias]: mock, __comp: null };

    try {
        vm.runInNewContext(code, ctx);
    } catch (e) {
        console.error(`  ! Parse hatası (${path.basename(mjsPath)}): ${e.message}`);
        return null;
    }

    if (typeof ctx.__comp !== 'function') return null;

    let tree;
    try {
        tree = ctx.__comp({ size: 24, color });
    } catch {
        return null;
    }

    return renderNode(tree);
}

/**
 * @untitledui/icons .mjs bileşenlerinden renk atanmış SVG dosyaları üretir.
 *
 * @param {string[]} iconNames  - İkon adları kebab-case (örn. "layout-grid-01")
 * @param {Record<string, string>} variants - { klasörAdı: '#RRGGBB' }
 */
function copyIconColorized(iconNames, variants) {
    const distDir = path.resolve('node_modules/@untitledui/icons/dist');

    if (!fs.existsSync(distDir)) {
        console.error('  ✗ @untitledui/icons bulunamadı. "npm install" çalıştır.');
        return;
    }

    for (const [variantName, color] of Object.entries(variants)) {
        const destDir = path.resolve(`wwwroot/icons/${variantName}`);
        fs.mkdirSync(destDir, { recursive: true });

        for (const name of iconNames) {
            const mjsFile = path.join(distDir, `${kebabToPascal(name)}.mjs`);

            if (!fs.existsSync(mjsFile)) {
                console.warn(`  ✗ İkon bulunamadı: ${name}`);
                continue;
            }

            const svg = extractSvgFromMjs(mjsFile, color);

            if (!svg) {
                console.warn(`  ✗ SVG üretilemedi: ${name}`);
                continue;
            }

            fs.writeFileSync(path.join(destDir, `${name}.svg`), svg, 'utf8');
            console.log(`  ✓ icons/${variantName}/${name}.svg`);
        }
    }
}

const nm  = 'node_modules';
const lib = 'wwwroot/lib';

console.log('\n[Grid.js]');
copy(`${nm}/gridjs/dist/gridjs.umd.js`,                            `${lib}/gridjs/gridjs.umd.js`);
copy(`${nm}/gridjs/dist/theme/mermaid.min.css`,                    `${lib}/gridjs/mermaid.min.css`);

console.log('\n[Swiper.js]');
copy(`${nm}/swiper/swiper-bundle.min.css`,                         `${lib}/swiper/swiper-bundle.min.css`);
copy(`${nm}/swiper/swiper-bundle.min.js`,                          `${lib}/swiper/swiper-bundle.min.js`);

console.log('\n[FilePond]');
copy(`${nm}/filepond/dist/filepond.min.css`,                       `${lib}/filepond/filepond.min.css`);
copy(`${nm}/filepond/dist/filepond.min.js`,                        `${lib}/filepond/filepond.min.js`);

console.log('\n[IMask.js]');
copy(`${nm}/imask/dist/imask.min.js`,                              `${lib}/imask/imask.min.js`);

console.log('\n[Toastify.js]');
copy(`${nm}/toastify-js/src/toastify.css`,                         `${lib}/toastify-js/toastify.css`);
copy(`${nm}/toastify-js/src/toastify.js`,                          `${lib}/toastify-js/toastify.js`);

console.log('\n[JustValidate]');
copy(`${nm}/just-validate/dist/just-validate.production.min.js`,  `${lib}/just-validate/just-validate.production.min.js`);

console.log('\n[Flatpickr]');
copy(`${nm}/flatpickr/dist/flatpickr.min.css`,                     `${lib}/flatpickr/flatpickr.min.css`);
copy(`${nm}/flatpickr/dist/flatpickr.min.js`,                      `${lib}/flatpickr/flatpickr.min.js`);
copy(`${nm}/flatpickr/dist/l10n/tr.js`,                            `${lib}/flatpickr/l10n/tr.js`);

console.log('\n[Alpine.js]');
copy(`${nm}/alpinejs/dist/cdn.min.js`,                             `${lib}/alpinejs/cdn.min.js`);

console.log('\n[TOM Select]');
copy(`${nm}/tom-select/dist/css/tom-select.default.min.css`,       `${lib}/tom-select/tom-select.default.min.css`);
copy(`${nm}/tom-select/dist/js/tom-select.complete.min.js`,        `${lib}/tom-select/tom-select.complete.min.js`);

console.log('\n[HTMX]');
copy(`${nm}/htmx.org/dist/htmx.min.js`,                           `${lib}/htmx/htmx.min.js`);

console.log('\n[Untitled UI Icons — Sidebar]');
copyIconColorized(
    [
        'layout-alt-02',
        'layout-grid-01',
        'file-04',
        'gift-01',
        'message-circle-01',
        'help-circle',
        'message-square-02',
        'receipt',
        'file-check-02',
        'settings-01',
        'bell-01',
    ],
    {
        dark:  '#1B4092',
        white: '#FFFFFF',
    }
);

console.log('\n[Untitled UI Icons — Avatar]');
copyIconColorized(['user-01'],      { navy: '#003F75' });
copyIconColorized(['chevron-down'], { blue: '#0056B3' });

console.log('\n[Untitled UI Icons — Theme Toggle]');
copyIconColorized(['sun'],   { sun:  '#F59E0B' });
copyIconColorized(['moon-01'], { moon: '#7A94B8' });

console.log('\nTamamlandı.\n');
