const fs   = require('fs');
const path = require('path');

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

console.log('\nTamamlandı.\n');
