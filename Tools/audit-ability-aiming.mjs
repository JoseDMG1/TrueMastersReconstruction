import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const base='UnityProject/Assets/Resources/photonprefabs';
const guids={HomingAttack:'36fad0212836cbcbcf01b212a7a3e1b2',LockToTarget:'530c057f45519d70869e3c440b5d1979',MoveToTarget:'6188c49cddb519977d3a74efd5165385'};
const profiles=[];
for(const name of fs.readdirSync(base).filter(n=>n.endsWith('.prefab'))){
 const file=path.join(base,name),raw=fs.readFileSync(file,'utf8');if(!Object.values(guids).some(g=>raw.includes(g)))continue;
 const parsed=parseUnity(raw);if(parsed.errors.length)throw Error(file+JSON.stringify(parsed.errors));
 for(const doc of parsed.documents){const type=Object.keys(guids).find(t=>guids[t]===doc.data.m_Script?.guid);if(!type)continue;
 profiles.push({resource:'photonprefabs/'+name.slice(0,-7),type,id:doc.document_id,fields:doc.data,sha256:crypto.createHash('sha256').update(raw).digest('hex')});}
}
const report={generatedUtc:new Date().toISOString(),evidenceType:'static-authored-unity-yaml',profiles,limitations:['Profile inventory is not runtime acceptance. Runtime component fixtures copy authored fields without spawning full damage prefabs. Video only shows sampled older elemental roster, not all twelve full movesets.']};
fs.writeFileSync('Evidence/ability-aiming-profiles.json',JSON.stringify(report,null,2));
fs.writeFileSync('Tools/unity-validation/AbilityAimProfiles.json',JSON.stringify({profiles:profiles.map(({resource,type,id,fields})=>({resource,type,id,delay:Number(fields.delay??0),horizontalSpeed:Number(fields.horizontalSpeed??40),verticalSpeed:Number(fields.verticalSpeed??0)}))},null,2));
console.log(JSON.stringify(Object.fromEntries(Object.keys(guids).map(t=>[t,profiles.filter(p=>p.type===t).length]))));

