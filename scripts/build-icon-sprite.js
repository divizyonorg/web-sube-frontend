const fs   = require('fs');
const path = require('path');

const ICONS_ROOT = path.resolve('node_modules/@untitled-ui/icons/icons');
const OUT_DIR    = path.resolve('wwwroot/icons');
const OUT_FILE   = path.join(OUT_DIR, 'sprite.svg');
const VARIANTS   = ['line', 'solid'];

function readSvgFiles(dir) {
    if (!fs.existsSync(dir)) return [];
    return fs.readdirSync(dir)
        .filter(f => f.endsWith('.svg'))
        .map(f => ({ name: path.basename(f, '.svg'), file: path.join(dir, f) }));
}

function extractSymbol(id, filePath) {
    const raw      = fs.readFileSync(filePath, 'utf8');
    const viewBox  = (raw.match(/viewBox="([^"]+)"/) || [])[1] ?? '0 0 24 24';
    const inner    = (raw.match(/<svg[^>]*>([\s\S]*?)<\/svg>/) || [])[1]?.trim() ?? '';
    return `  <symbol id="${id}" viewBox="${viewBox}">${inner}</symbol>`;
}

fs.mkdirSync(OUT_DIR, { recursive: true });

const symbols = [];

for (const variant of VARIANTS) {
    const variantDir = path.join(ICONS_ROOT, variant);
    const icons      = readSvgFiles(variantDir);

    if (icons.length === 0) {
        console.warn(`  ✗ Klasör bulunamadı: ${variantDir}`);
        continue;
    }

    for (const { name, file } of icons) {
        const id = variant === 'line' ? name : `${variant}-${name}`;
        symbols.push(extractSymbol(id, file));
    }

    console.log(`  ✓ [${variant}] ${icons.length} ikon eklendi`);
}

const sprite = `<svg xmlns="http://www.w3.org/2000/svg" style="display:none">\n${symbols.join('\n')}\n</svg>\n`;
fs.writeFileSync(OUT_FILE, sprite, 'utf8');

console.log(`\n  ✓ sprite.svg → ${OUT_FILE} (${symbols.length} sembol)\n`);
