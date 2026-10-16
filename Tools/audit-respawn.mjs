import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const base=path.resolve(import.meta.dirname,'..'), assets=path.join(base,'UnityProject/Assets');
const sha=x=>crypto.createHash('sha256').update(x).digest('hex');
const rows=[];
for(const file of fs.readdirSync(path.join(assets,'AnimatorController')).filter(x=>x.endsWith('.controller'))){
 const rel='AnimatorController/'+file,text=fs.readFileSync(path.join(assets,rel),'utf8'),parsed=parseUnity(text);
 if(parsed.errors.length) throw Error(JSON.stringify(parsed.errors));
 const lookup=new Map(parsed.documents.map(x=>[x.document_id,x]));
 const states=parsed.documents.filter(x=>x.type==='AnimatorState');
 const selected=states.filter(x=>/^(Falling|Death)/.test(x.data.m_Name));
 const refs=s=>s.data.m_StateMachineBehaviours?.map(x=>lookup.get(x.fileID)).map(x=>({id:x?.document_id,script:x?.data.m_Script,death:x?.data.death}));
 rows.push({file:rel,sha256:sha(text),states:selected.map(s=>({id:s.document_id,name:s.data.m_Name,speed:s.data.m_Speed,motion:s.data.m_Motion,behaviours:refs(s),transitions:s.data.m_Transitions.map(r=>lookup.get(r.fileID)).map(t=>({id:t?.document_id,target:lookup.get(t?.data.m_DstState?.fileID)?.data.m_Name,conditions:t?.data.m_Conditions,hasExitTime:t?.data.m_HasExitTime,exitTime:t?.data.m_ExitTime,duration:t?.data.m_TransitionDuration}))}))});
}
const clips=[];
for(const file of fs.readdirSync(path.join(assets,'AnimationClip')).filter(x=>/^Falling.*\.anim$/.test(x))){
 const rel='AnimationClip/'+file,text=fs.readFileSync(path.join(assets,rel),'utf8');
 // Isolate the top-level event map using its indentation boundary, then parse with the same YAML owner.
 const start=text.indexOf('\n  m_Events:');
 const body='%YAML 1.1\n--- !u!74 &7400000\nAnimationClip:\n'+text.slice(start+1);
 const parsed=parseUnity(body); if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));
 clips.push({file:rel,sha256:sha(text),events:parsed.documents[0]?.data.m_Events});
}
const out={generatedUtc:new Date().toISOString(),status:'observed_asset_structure_not_runtime',controllers:rows,clips};
fs.writeFileSync(path.join(base,'Evidence/respawn-animation-audit.json'),JSON.stringify(out,null,2));
console.log(JSON.stringify({controllers:rows.length,clips:clips.length,respawnEvents:clips.filter(x=>x.events?.some(e=>e.functionName==='respawn')).length}));
