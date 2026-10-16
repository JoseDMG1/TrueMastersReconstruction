import fs from 'node:fs';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const names=['DisableColliderOnCollition','DisableColliderOnCollition_LM'];
const guid=name=>fs.readFileSync(`UnityProject/Assets/Scripts/Assembly-CSharp/${name}.cs.meta`,'utf8').match(/^guid: (.+)$/m)[1];
const owners=new Map(names.map(name=>[guid(name),name]));
const prefabs=[];
for(const file of fs.readdirSync('UnityProject/Assets',{recursive:true}).filter(f=>f.endsWith('.prefab'))){
 const path='UnityProject/Assets/'+file.replaceAll('\\','/'),text=fs.readFileSync(path,'utf8');
 if(![...owners.keys()].some(g=>text.includes(g))) continue;
 const parsed=parseUnity(text); if(parsed.errors.length) throw Error(JSON.stringify(parsed.errors));
 const byId=new Map(parsed.documents.map(d=>[d.document_id,d]));
 const components=parsed.documents.filter(d=>owners.has(d.data.m_Script?.guid)).map(d=>{
  const own=byId.get(d.data.m_GameObject?.fileID),box=byId.get(d.data.col?.fileID);
  return {type:owners.get(d.data.m_Script.guid),id:d.document_id,owner:own?.data.m_Name,layer:own?.data.m_Layer,tag:own?.data.m_TagString,onlyByPlayers:d.data.onlyByPlayers??null,referencedBox:d.data.col??null,referencedBoxType:box?.type??null};
 });
 prefabs.push({path,sha256:sha(path),components});
}
const nativePath='Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so';
const runtimePath=process.argv[2],runtime=runtimePath?read(runtimePath):null;
const launch=runtimePath?read(runtimePath.replace(/\.json$/,'-launch.json')):null;
const buildPath=process.argv[3];
const observations=runtime?.observations.filter(o=>o.step.startsWith('collider-disable-'));
if(runtime&&(runtime.result!=='passed'||runtime.errors.length||observations.filter(o=>o.step==='collider-disable-case').length!==14)) throw Error('Expected passing fourteen-case physics report');
const result={generatedUtc:new Date().toISOString(),classification:'native-static-contract-with-bounded-reconstruction-physics',native:{path:nativePath,sha256:sha(nativePath),addressMap:'Evidence/cpp2il-native-address-map.json',tool:'local Capstone via Tools/disassemble-type.ps1; retained Cpp2IL ISIL supplies API names; not a connected REA MCP result'},sources:names.map(name=>{const path=`UnityProject/Assets/Scripts/Assembly-CSharp/${name}.cs`,listing=`Evidence/native-disassembly-${name}.txt`;return {path,sha256:sha(path),listing,listingSha256:sha(listing)};}),resolvedSlots:{literals:read('Evidence/collider-disable-literals.json'),generics:read('Evidence/collider-disable-generics.json')},contracts:{DisableColliderOnCollition:{triggerVA:'0x1228CF8',filter:'incoming layer must be 8 or 9, different from own GameObject layer',onlyByPlayers:'when true, own GetComponent<Description>() must exist and Type must NOT equal Char; incoming Description is never read',effect:'disable referenced col; no fallback to own BoxCollider',adaptation:'skip a null col to avoid native null fault; original LeftWaterAtk2 has col fileID 0',start:'0x1228CF4 RET'},DisableColliderOnCollition_LM:{triggerVA:'0x1228E74',filter:'own GetComponent<BoxCollider>().gameObject tag Attack or Player; incoming collider ignored',effect:'disable own BoxCollider',start:'0x1228E70 RET'}},prefabs,runtime:runtime?{path:runtimePath,sha256:sha(runtimePath),result:runtime.result,errors:runtime.errors,observations,limitation:runtime.limitation}:null,limits:['Synthetic collision fixtures establish reconstructed branch behavior, not natural attack aiming or complete original runtime parity.','Layer collision pairs temporarily enabled/restored by fixtures. Serialized prefab references are inspected without asset edits.','Natural FlowerBeam activation and full Android/online behavior remain unresolved.']};
if(runtime) result.runtime.gameplayAssemblySha256=launch.GameplayAssemblySha256;
if(buildPath) result.build={path:buildPath,sha256:sha(buildPath),report:read(buildPath)};
result.adaptations=[{reason:'Skip a missing serialized col rather than reproducing a null exception; do not choose a fallback',originalPrefab:'Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets/Resources/photonprefabs/LeftWaterAtk2.prefab',originalPrefabSha256:sha('Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets/Resources/photonprefabs/LeftWaterAtk2.prefab'),field:'col fileID 0',nativeParity:false}];
fs.writeFileSync('Evidence/collider-disable-native-contract.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({prefabs:prefabs.length,components:prefabs.flatMap(p=>p.components).length,cases:observations?.filter(o=>o.step==='collider-disable-case').length??0,runtime:runtime?.result??'not selected'}));
