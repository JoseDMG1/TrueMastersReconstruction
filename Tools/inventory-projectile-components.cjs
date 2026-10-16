const fs = require('fs');
const path = require('path');
const base = path.resolve(__dirname, '..');
const assets = path.join(base, 'UnityProject/Assets');
function walk(directory) {
 return fs.readdirSync(directory, { withFileTypes: true }).flatMap(entry => entry.isDirectory()
  ? walk(path.join(directory, entry.name)) : [path.join(directory, entry.name)]);
}
// Unity m_Script scalar references only. This inventories references, not runtime instances.
const counts = new Map();
const prefabs = walk(assets).filter(file => file.endsWith('.prefab'));
for (const file of prefabs) {
 const text = fs.readFileSync(file, 'utf8');
 for (const [, guid] of text.matchAll(/m_Script: \{fileID: 11500000, guid: ([a-f0-9]+), type: 3\}/g)) {
  let row = counts.get(guid);
  if (!row) counts.set(guid, row = { component_references: 0, prefab_files: new Set() });
  row.component_references++; row.prefab_files.add(path.relative(assets, file).replaceAll('\\', '/'));
 }
}
const scripts = path.join(assets, 'Scripts/Assembly-CSharp');
const rows = fs.readdirSync(scripts).filter(name => name.endsWith('.cs')).map(name => {
 const meta = fs.readFileSync(path.join(scripts, name + '.meta'), 'utf8');
 const guid = meta.match(/^guid: ([a-f0-9]+)$/m)?.[1];
 const count = counts.get(guid);
 return { script: name, guid, component_references: count?.component_references ?? 0,
  prefab_file_count: count?.prefab_files.size ?? 0, prefab_files: [...(count?.prefab_files ?? [])] };
}).filter(row => row.component_references).sort((a,b) => b.component_references - a.component_references);
const result = { generatedUtc: new Date().toISOString(), examined_prefab_files: prefabs.length,
 scope: 'Literal m_Script references for root Assembly-CSharp scripts, no runtime execution or behavior-completeness claim.', rows };
const output = path.join(base, 'Evidence/projectile-component-reference-inventory.json');
fs.writeFileSync(output, JSON.stringify(result, null, 2));
console.log(JSON.stringify({ output, examined_prefab_files: prefabs.length, scripts: rows.length,
 top: rows.slice(0, 12).map(({prefab_files, ...row}) => row) }, null, 2));
