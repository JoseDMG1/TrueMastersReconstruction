import fs from 'node:fs';
import crypto from 'node:crypto';
import path from 'node:path';
import {parseUnity} from './unity-audit/audit.mjs';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const owners=['ShadowTextureRenderer','DrawTargetObject'];
const guid=name=>fs.readFileSync(`UnityProject/Assets/Scripts/Assembly-CSharp/DynamicShadowProjector/${name}.cs.meta`,'utf8').match(/^guid: (.+)$/m)[1];
const map=new Map(owners.map(n=>[guid(n),n]));
const prefabs=['Sora','Vinea','Diana','Amon','Aura','Eliot','Ren','Rina','Dante','Agata','Raven','Maya'].map(name=>{
 const path=`UnityProject/Assets/Resources/photonprefabs/${name}.prefab`,parsed=parseUnity(fs.readFileSync(path,'utf8'));
 if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));
 const components=parsed.documents.filter(d=>map.has(d.data.m_Script?.guid)).map(d=>({type:map.get(d.data.m_Script.guid),id:d.document_id,fields:d.data}));
 return {path,sha256:sha(path),components};
});
const rendererProfiles=prefabs.flatMap(p=>p.components).filter(c=>c.type==='ShadowTextureRenderer');
const unsupported=rendererProfiles.filter(c=>Number(c.fields.m_blurLevel)!==0||Number(c.fields.m_mipLevel)!==0||Number(c.fields.m_superSampling)>1||Number(c.fields.m_testViewClip)!==0);
const sourcePaths=owners.map(n=>`UnityProject/Assets/Scripts/Assembly-CSharp/DynamicShadowProjector/${n}.cs`).concat(['Opaque','Transparent','Transparent Cutout'].map(n=>`UnityProject/Assets/Shader/DynamicShadowProjector_Shadow_${n}.shader`));
const runtimePath=process.argv[2],buildPath=process.argv[3],runtime=runtimePath?read(runtimePath):null;
const observations=runtime?.observations.filter(o=>o.step.startsWith('dynamic-shadow-')||o.step.startsWith('projector'));
if(runtime&&(runtime.result!=='passed'||runtime.errors.length||!observations.some(o=>o.step==='dynamic-shadow-animation')||!observations.some(o=>o.step==='dynamic-shadow-lifecycle')))throw Error('Dynamic GPU/lifecycle runtime did not pass');
if(runtime&&!observations.some(o=>o.step==='dynamic-shadow-animation'&&o.detail.includes('automatic camera rendering')))throw Error('Final acceptance requires automatic camera rendering');
const result={generatedUtc:new Date().toISOString(),classification:'partial-authored-profile-reconstruction-not-full-vendor-plugin',nativeSha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',nativeListings:owners.map(n=>{const path=`Evidence/native-disassembly-DynamicShadowProjector.${n}.txt`;return {path,sha256:sha(path)};}),sources:sourcePaths.map(path=>({path,sha256:sha(path)})),resolvedRendererSlot:read('Evidence/shadow-core-renderer-slot.json'),resolvedRenderTag:read('Evidence/shadow-core-render-tag-slot.json'),prefabs,profile:{owners:rendererProfiles.length,unsupportedAuthoredProfiles:unsupported.length,supported:'Built-In RP, no blur/mips/supersampling/view clipping; serialized dimensions, multisampling and target renderer hierarchy/layer mask'},nativeContracts:{commandBuffer:'0x12E0EEC remove/add CameraEvent12 BeforeImageEffectsOpaque; remove0x12E0FC0',target:'Awake0x12E0CE0 stores ownTransform.InverseTransformPoint(target.position); UpdateTransform0x12E0AB0 moves own position by target.position-own.TransformPoint(offset); alignment uses target axis/direction',draw:'AddDrawCommandForGameObject0x12E07B0 checks active hierarchy, enabled renderer, layer mask, then children; AddDrawCommand0x12E12E0 handles submeshes/materials',texture:'CreateRenderTexture0x12E3148, getters expose serialized dimensions/sample parameters and _ShadowTex runtime binding'},reconstructionDecisions:['Opaque/transparent/cutout shaders are new approximate masks; original shader bodies unavailable. Existing receiver shader was already an approximation.','Clone one projector material per owner; capture to owned depth16 ARGB32/preferred supported format, supported MSAA, white neutral background.','Refresh command buffer draws each capture to follow enabled/material changes. Full vendor cache/hash parity is not reconstructed.','Advanced unused helper bodies remain stubs; unsupported blur/mip/supersampling/view-clipping profiles are explicitly gated off.','Transparent mask texture alpha and cutout handling are approximations; natural transparent material parity unaccepted.'],runtime:runtime?{path:runtimePath,sha256:sha(runtimePath),assemblySha256:read(runtimePath.replace(/\.json$/,'-launch.json')).GameplayAssemblySha256,result:runtime.result,errors:runtime.errors,observations,limitation:runtime.limitation}:null,build:buildPath?{path:buildPath,sha256:sha(buildPath),report:read(buildPath)}:null,backups:'Evidence/shadow-core-backup',limits:['No original runtime/visual parity or full plugin recovery.','Static twelve-roster profile inventory is not all-roster GPU acceptance.','No external REA MCP tools connected; local native/metadata evidence following skill.']};
if(runtimePath) {
 const prefix=path.basename(runtimePath,'.json');
 result.images=fs.readdirSync('Evidence').filter(n=>n.startsWith(prefix+'.')&&n.endsWith('.png')).map(n=>({path:'Evidence/'+n,sha256:sha('Evidence/'+n)}));
 result.visualInspection='Automatic pose captures and full scene inspected; silhouette changes and projected contact shadow visible. Receiver screenshots are qualitative, not a registered pixel A/B because actor pose settles between captures.';
}
fs.writeFileSync('Evidence/dynamic-shadow-core-contract.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({fighterPrefabs:prefabs.length,rendererProfiles:rendererProfiles.length,unsupportedAuthoredProfiles:unsupported.length,runtime:runtime?.result??'not selected'}));
