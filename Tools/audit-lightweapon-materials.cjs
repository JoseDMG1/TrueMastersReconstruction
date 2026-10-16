const fs = require('fs');
const path = require('path');
const crypto = require('crypto');

(async () => {
  const { parseUnity } = await import('./unity-audit/audit.mjs');
  const root = path.resolve(__dirname, '..');
  const inputs = [
    'UnityProject/Assets/Scenes/Character Selection.unity',
    'UnityProject/Assets/Resources/photonprefabs/Agata.prefab',
    'UnityProject/Assets/Resources/photonprefabs/Dante.prefab',
    'Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets/Scenes/Character Selection.unity'
  ];
  const reports = inputs.map(input => {
    const text = fs.readFileSync(path.join(root, input), 'utf8');
    const parsed = parseUnity(text);
    if (parsed.errors.length) throw Error(JSON.stringify(parsed.errors));
    const byId = new Map(parsed.documents.map(d => [d.document_id, d]));
    const owners = parsed.documents.filter(d => d.data.m_Script?.guid === '5c6b9e4428b4344a03043b19dc58006f').map(d => ({
      component_id: d.document_id,
      gameobject_name: byId.get(d.data.m_GameObject.fileID)?.data.m_Name,
      serialized_material_lists: { sword: d.data.sword, shield: d.data.shield },
      source_renderers: ['swordMesh', 'shieldMesh'].flatMap(field => d.data[field].map(ref => {
        const renderer = byId.get(ref.fileID);
        return { field, renderer_id: ref.fileID, class: renderer?.type, materials: renderer?.data.m_Materials };
      }))
    }));
    return { input, sha256: crypto.createHash('sha256').update(text).digest('hex'), owners };
  });
  const result = { status: 'observed-static', limitations: ['Does not execute Unity or prove event timing.'], reports };
  fs.writeFileSync(path.join(root, 'Evidence/lightweapon-material-boundary-audit.json'), JSON.stringify(result, null, 2));
  console.log(JSON.stringify(reports.map(r => ({ input: r.input, owners: r.owners.map(o => ({ name: o.gameobject_name, sword: o.serialized_material_lists.sword, renderer_count: o.source_renderers.length })) })), null, 2));
})();
