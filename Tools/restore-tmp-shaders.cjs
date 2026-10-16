// Replace only confirmed exported placeholder shader bodies; retain asset GUIDs.
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = path.resolve(__dirname, '..');
const resources = path.join(root, 'Evidence/official-tmp-resources');
const destination = path.join(root, 'UnityProject/Assets/Shader');
const hash = data => crypto.createHash('sha256').update(data).digest('hex');
const pairs = [
  ['fe393ace9b354375a9cb14cdbbc28be4', 'TextMeshPro_Mobile_Distance Field.shader'],
  ['68e6db2ebdc24f95958faec2be5558d6', 'TextMeshPro_Distance Field.shader'],
  ['cf81c85f95fe47e1a27f6ae460cf182c', 'TextMeshPro_Sprite.shader'],
  ['3997e2241185407d80309a82f9148466', 'TMPro_Properties.cginc'],
  ['407bc68d299748449bbf7f48ee690f8d', 'TMPro.cginc'],
];
const prepared = pairs.map(([id, name]) => {
  const source = path.join(resources, id, 'asset');
  const sourceName = fs.readFileSync(path.join(resources, id, 'pathname'), 'utf8').trim();
  const file = path.join(destination, name);
  const before = fs.existsSync(file) ? fs.readFileSync(file) : null;
  const after = fs.readFileSync(source);
  if (before && before.equals(after)) return { file, sourceName, before, after, change: false };
  if (before && name.endsWith('.shader')) {
    const oldName = before.toString().match(/Shader\s+"([^"]+)"/)?.[1];
    const newName = after.toString().match(/Shader\s+"([^"]+)"/)?.[1];
    if (!before.toString().includes('//DummyShaderTextExporter') || oldName !== newName)
      throw Error('Refusing to replace non-placeholder or mismatched shader: ' + file);
  } else if (before) throw Error('Conflicting include file: ' + file);
  return { file, sourceName, before, after, change: true };
});
const stamp = new Date().toISOString().replace(/[:.]/g, '-');
const backup = path.join(root, 'WorkingBackups', 'tmp-shaders-' + stamp);
const report = { generatedUtc: new Date().toISOString(), mode: process.argv.includes('--apply') ? 'apply' : 'plan',
  source: 'Official TMP Essential Resources.unitypackage bundled with installed Unity6000.6.5f1/UGUI2.6.0', backup,
  entries: prepared.map(p => ({ file: p.file, source: p.sourceName, beforeSha256: p.before && hash(p.before), afterSha256: hash(p.after), change: p.change })),
  preservation: 'Existing .meta GUIDs, materials, fonts and serialized references retained. Original APK exports untouched.',
  limitation: 'Official compatible shader replacement; no assertion of exact original compiled shader equivalence. Requires graphics player validation.' };
if (report.mode === 'apply') {
  fs.mkdirSync(backup, { recursive: true });
  for (const p of prepared.filter(p => p.change)) {
    if (p.before) fs.writeFileSync(path.join(backup, path.basename(p.file)), p.before);
    fs.writeFileSync(p.file, p.after);
  }
  fs.writeFileSync(path.join(root, 'Evidence/tmp-shader-restoration.json'), JSON.stringify(report, null, 2));
}
console.log(JSON.stringify(report, null, 2));
