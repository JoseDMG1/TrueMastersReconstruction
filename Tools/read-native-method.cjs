const fs = require('fs');
const path = require('path');
const base = path.resolve(__dirname, '..');
const [type, ...methods] = process.argv.slice(2);
const map = JSON.parse(fs.readFileSync(path.join(base, 'Evidence/cpp2il-native-address-map.json')));
const names = new Map(map.entries.map(e => [Number(e.native_address), e.method_full_name]));
const dump = fs.readFileSync(path.join(base, `Evidence/native-disassembly-${type}.txt`), 'utf8');
for (const block of dump.split(/(?=Method: )/)) {
 if (methods.length && !methods.some(m => block.split('\n')[0].includes(m + '('))) continue;
 console.log(block.replace(/\bb[l]? #(0x[0-9a-f]+)/g, (s, a) => s + (names.has(Number(a)) ? ' // ' + names.get(Number(a)) : '')));
}
