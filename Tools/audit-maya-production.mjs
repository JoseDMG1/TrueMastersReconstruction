import fs from 'node:fs';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const sha = p => crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const scripts = ['sumonOnCollider', 'ZigzagController'];
const guids = new Map(scripts.map(name => [fs.readFileSync(`UnityProject/Assets/Scripts/Assembly-CSharp/${name}.cs.meta`,'utf8').match(/^guid: (.+)$/m)[1], name]));
const prefabs = [];
const metaFiles = fs.readdirSync('UnityProject/Assets',{recursive:true}).filter(f => f.endsWith('.prefab.meta'));
for (const name of ['LeafFrontHit','LeafAtkL','LeafAtkR','FlowerBeam','FloorBeam','FloorBeam 1']) {
    const path = `UnityProject/Assets/Resources/photonprefabs/${name}.prefab`;
    const parsed = parseUnity(fs.readFileSync(path,'utf8'));
    if (parsed.errors.length) throw Error(JSON.stringify(parsed.errors));
    const components = parsed.documents.filter(d => guids.has(d.data.m_Script?.guid))
        .map(d => ({type:guids.get(d.data.m_Script.guid),id:d.document_id,fields:d.data}));
    if (!components.length) throw Error('Expected native producer/motion owner on '+name);
    for (const component of components) {
        if (!component.fields.sumon?.guid) continue;
        const guid = component.fields.sumon.guid;
        const matches = metaFiles.filter(f => fs.readFileSync('UnityProject/Assets/'+f,'utf8').includes('guid: '+guid));
        if (matches.length !== 1) throw Error('Ambiguous child prefab '+guid);
        component.childPrefab = matches[0].replace(/\.meta$/,'');
    }
    prefabs.push({path,sha256:sha(path),components});
}
const runtimePath = process.argv[2];
const runtime = runtimePath ? JSON.parse(fs.readFileSync(runtimePath,'utf8')) : null;
const result = {
    generatedUtc:new Date().toISOString(), classification:'observed-static-native-and-serialized-contract',
    nativeSha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',
    sources:scripts.map(name => {const path=`UnityProject/Assets/Scripts/Assembly-CSharp/${name}.cs`, nativeListing=`Evidence/native-disassembly-${name}.txt`; return {path,sha256:sha(path),nativeListing,nativeListingSha256:sha(nativeListing)};}),
    contracts:{
        sumonOnCollider:{triggerVA:'0x12C70C8',localVA:'0x12C7340',networkVA:'0x12C7200',filter:'Collider.GetComponent<Description>() != null; no layer, type or damage threshold',local:'Instantiate child at incoming collider transform position/rotation with no parent or ownership event',network:'IsConnected && own PhotonView.IsMine; Instantiate Path.Combine(PhotonPrefabs,child.name) at target pose; backend not accepted'},
        ZigzagController:{startVA:'0x12DD8B4',updateVA:'0x12DD8C8',constructorVA:'0x12DD9B4',defaults:{moveDistance:1,switchTime:1,direction:1},start:'timer=switchTime*.5',update:'timer-=deltaTime; position+=right*moveDistance*direction; if timer<=0 negate direction and reset timer=switchTime',motion:'per Update, no deltaTime multiplier, move then reverse; no overshoot carry'}
    },prefabs,
    validation:{csharpCompile:'passed using installed Roslyn and retained Unity player response file; full gameplay sources plus helper',log:'Evidence/maya-production-compile.log',editorBuild:JSON.parse(fs.readFileSync('Evidence/windows-build-validation.json','utf8')),license:'Initial exit198 resolved on retry after user confirmed Personal license in Hub; no licensing configuration changed',runtime:runtime ? {path:runtimePath,sha256:sha(runtimePath),result:runtime.result,errors:runtime.errors,observations:runtime.observations.filter(o=>o.step.startsWith('maya-')),limitation:runtime.limitation} : 'Not selected; supply an explicit retained probe JSON as argument'},
    limitations:['Synthetic remote targets/layer0 self collisions; FlowerBeam root collider explicitly enabled in fixture, natural flower activation not accepted.','Native network branch reimplemented against exported Photon API stubs; no service or online behavior restored.','No original runtime or graphics parity claim.']
};
fs.writeFileSync('Evidence/maya-production-native-contract.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({prefabs:prefabs.map(p=>({path:p.path,children:p.components.map(c=>c.childPrefab).filter(Boolean)})),validation:result.validation}));
