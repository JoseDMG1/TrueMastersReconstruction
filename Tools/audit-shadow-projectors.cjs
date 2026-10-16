const fs = require('fs');
const path = require('path');
const crypto = require('crypto');
(async () => {
  const { parseUnity } = await import('./unity-audit/audit.mjs');
  const root = path.resolve(__dirname, '..');
  const hash = text => crypto.createHash('sha256').update(text).digest('hex');
  const shaderPath = 'UnityProject/Assets/Shader/DynamicShadowProjector_Projector_Dynamic_Shadow With Linear Falloff.shader';
  const shader = fs.readFileSync(path.join(root, shaderPath), 'utf8');
  const materialPath = 'UnityProject/Assets/Material/BlobShadowProjector.mat';
  const material = fs.readFileSync(path.join(root, materialPath), 'utf8');
  const materialData = parseUnity(material).documents[0].data;
  const fighters = [];
  for (const name of ['Sora','Vinea','Diana','Amon','Aura','Eliot','Ren','Rina','Dante','Agata','Raven','Maya']) {
    const input = 'UnityProject/Assets/Resources/photonprefabs/' + name + '.prefab';
    const text = fs.readFileSync(path.join(root, input), 'utf8');
    const parsed = parseUnity(text);
    if (parsed.errors.length) throw Error(JSON.stringify(parsed.errors));
    const byId = new Map(parsed.documents.map(d => [d.document_id, d]));
    fighters.push({name, input, sha256:hash(text), projectors: parsed.documents.filter(d => d.type === 'Projector').map(d => ({
      id: d.document_id, name: byId.get(d.data.m_GameObject.fileID)?.data.m_Name,
      enabled: d.data.m_Enabled, material: d.data.m_Material,
      near: d.data.m_NearClipPlane, far: d.data.m_FarClipPlane,
      orthographic: d.data.m_Orthographic, size: d.data.m_OrthographicSize,
      ignore_layers: d.data.m_IgnoreLayers
    }))});
  }
  const result = {status:'observed-static; runtime causation awaits parent A/B', shader:{input:shaderPath,sha256:hash(shader),dummy_exporter:shader.includes('DummyShaderTextExporter'),fragment_constant_white:shader.includes('return float4(1.0, 1.0, 1.0, 1.0)'),blend_declared:/^\s*Blend\s/m.test(shader),zwrite_off:/^\s*ZWrite Off/m.test(shader),projector_transform:shader.includes('unity_Projector')}, material:{input:materialPath,sha256:hash(material),data:materialData},fighters};
  fs.writeFileSync(path.join(root, 'Evidence/shadow-projector-static-audit.json'), JSON.stringify(result,null,2));
  console.log(JSON.stringify({shader:result.shader,counts:fighters.map(f=>({name:f.name,projectors:f.projectors.length}))},null,2));
})();
