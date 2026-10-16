import fs from 'node:fs';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const names=['Sora','Vinea','Diana','Amon','Aura','Eliot','Ren','Rina','Dante','Agata','Raven','Maya'];
const source='UnityProject/Assets/Scripts/Assembly-CSharp/ShadowController.cs';
const guid=fs.readFileSync(source+'.meta','utf8').match(/^guid: (.+)$/m)[1];
const prefabs=names.map(name=>{const path=`UnityProject/Assets/Resources/photonprefabs/${name}.prefab`,r=parseUnity(fs.readFileSync(path,'utf8'));if(r.errors.length)throw Error(JSON.stringify(r.errors));return {name,path,sha256:sha(path),controllers:r.documents.filter(d=>d.data.m_Script?.guid===guid).map(d=>({id:d.document_id,gameObject:d.data.m_GameObject}))};});
const runtimePath=process.argv[2],buildPath=process.argv[3],runtime=runtimePath?read(runtimePath):null;
const observations=runtime?.observations.filter(o=>o.step.startsWith('shadow-')||o.step.startsWith('dynamic-shadow-')||o.step==='fixture-restored');
if(runtime) {
 const accepted=observations.filter(o=>o.step==='shadow-roster-accepted').map(o=>o.detail);
 if(runtime.result!=='passed'||runtime.errors.length||accepted.length!==12||names.some(n=>!accepted.includes(n)))throw Error('Expected passing twelve-fighter roster report');
 if(observations.filter(o=>o.step==='shadow-quality-case').length!==60||observations.filter(o=>o.step==='dynamic-shadow-animation').length!==12)throw Error('Missing quality/animation coverage');
 if(observations.filter(o=>o.step==='shadow-quality-follow').length!==12||observations.filter(o=>o.step==='shadow-quality-case').some(o=>!o.detail.includes('forwardMatchesLight=True')))throw Error('Missing native direction/follow coverage');
}
const listing='Evidence/native-disassembly-ShadowController.txt';
const result={generatedUtc:new Date().toISOString(),classification:'native-controller-reconstruction-with-bounded-roster-GPU',nativeSha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',source:{path:source,sha256:sha(source)},graphicsPublisher:{path:'UnityProject/Assets/Scripts/Assembly-CSharp/GraphicsManager.cs',sha256:sha('UnityProject/Assets/Scripts/Assembly-CSharp/GraphicsManager.cs')},listing:{path:listing,sha256:sha(listing)},slots:read('Evidence/shadow-quality-slots.json'),configSlot:read('Evidence/shadow-quality-config-slot.json'),contract:{startVA:'0x12BD11C',destroyVA:'0x12BD2D8',changeVA:'0x12BD260',start:'subscribe ChangeShadow to GraphicsManager.graphicsChange; GetComponent<DrawTargetObject>; targetDirection=GameObject.FindGameObjectWithTag(MainLight).transform; apply preference',quality:'gameObject.SetActive(PlayerPrefs.GetInt(userQualityGraphics,0)==1); exact equality, not threshold',destroy:'unsubscribe same callback'},adaptation:'retain publisher identity for symmetric cleanup; tolerate missing publisher/config/light rather than native null fault, without inventing replacement objects',prefabs,runtime:runtime?{path:runtimePath,sha256:sha(runtimePath),assemblySha256:read(runtimePath.replace(/\.json$/,'-launch.json')).GameplayAssemblySha256,result:runtime.result,errors:runtime.errors,observations,limitation:runtime.limitation}:null,build:buildPath?{path:buildPath,sha256:sha(buildPath),report:read(buildPath)}:null,limits:['Quality matrix uses native preference plus actual graphicsChange event dispatch; it does not click MIN/MED/MAX buttons or establish resolution-switching behavior.','Controlled BasicAttack poses/disabled animation events; not full movesets, original visual parity or performance.','Approximate mask/receiver shaders and partial basic vendor profile only; advanced shadow features remain incomplete.','Earlier Graphics normal branch did not call CaptureFixture; old claims of its full save/preference restoration were unsupported. New Graphics branch explicitly captures and ShadowRoster inherits captured CombatTrip fixture including quality/frame-cap preference presence/values.','No connected REA server output; local retained ARM64, metadata and YAML following REA skill.']};
result.captureOrientation={nativeVA:'0x12E0AB0',direction:'targetDirection.forward when present, otherwise own transform.forward',up:'target right/up/forward according to textureAlignment, including target.up for None with a direction',follow:'Awake caches local target offset; OnEnable does not recache it. Controlled displacement while inactive must retain offset on reactivation.'};
if(runtimePath) {
 const prefix=runtimePath.replace(/^.*[\\/]/,'').replace(/\.json$/,'');
 result.images=fs.readdirSync('Evidence').filter(n=>n.startsWith(prefix+'.')&&n.endsWith('.png')).map(n=>({path:'Evidence/'+n,sha256:sha('Evidence/'+n)}));
}
fs.writeFileSync('Evidence/shadow-quality-roster-contract.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({prefabs:prefabs.length,controllers:prefabs.flatMap(p=>p.controllers).length,runtime:runtime?.result??'not selected'}));
