import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
import {parseDocument} from './unity-audit/node_modules/yaml/dist/index.js';
const root=process.cwd(),assets=path.join(root,'UnityProject/Assets');
const started=performance.now(),cpuStarted=process.cpuUsage();
const roster=['Sora','Vinea','Diana','Amon','Aura','Eliot','Ren','Rina','Dante','Agata','Raven','Maya'];
const walk=p=>fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);
const files=walk(assets),guids=new Map(),cache=new Map();
const relative=p=>path.relative(root,p).replaceAll('\\','/');
const sha=raw=>crypto.createHash('sha256').update(raw).digest('hex');
for(const file of files.filter(f=>f.endsWith('.meta'))){const doc=parseDocument(fs.readFileSync(file,'utf8'),{schema:'failsafe',uniqueKeys:true});if(doc.errors.length)throw Error('Malformed meta '+file);const guid=doc.toJS()?.guid;if(guid)guids.set(guid,file.slice(0,-5));}
function asset(file){
 if(cache.has(file))return cache.get(file);
 const raw=fs.readFileSync(file,'utf8');
 // Curves are outside this event/route audit. Project only complete top-level
 // name/events fields before YAML parsing, retaining the full source digest.
 let selected=raw;
 if(file.endsWith('.anim')){
  const lines=raw.split(/\r?\n/),header=lines.find(l=>/^--- !u!74 &/.test(l));
  if(!header)throw Error('Missing AnimationClip identity '+file);
  const nameIndex=lines.findIndex(l=>/^  m_Name:/.test(l));
  const eventIndex=lines.findIndex(l=>/^  m_Events:/.test(l));
  if(nameIndex<0||eventIndex<0)throw Error('Missing clip field '+file);
  let end=eventIndex+1;
  while(end<lines.length&&!/^  [A-Za-z_][^:]*:/.test(lines[end]))end++;
  selected=header+'\nAnimationClip:\n'+lines[nameIndex]+'\n'+lines.slice(eventIndex,end).join('\n');
 }
 const p=parseUnity(selected);
 if(p.errors.length)throw Error(file+JSON.stringify(p.errors));
 const result={file:relative(file),sha256:sha(raw),documents:p.documents,byId:new Map(p.documents.map(d=>[d.document_id,d]))};
 cache.set(file,result);return result;
}
function source(ref,owner){if(!ref||ref.fileID==='0')return null;if(!ref.guid||/^0+$/.test(ref.guid))return {asset:owner,id:ref.fileID};const p=guids.get(ref.guid);return p?{asset:asset(p),id:ref.fileID}:null;}
function motions(ref,owner,visited=new Set()){
 const s=source(ref,owner);if(!s)return [];
 const identity=s.asset.file+'#'+s.id;if(visited.has(identity))throw Error('Motion cycle '+identity);visited=new Set([...visited,identity]);
 const d=s.asset.byId.get(s.id);if(!d)return [{kind:'unknown',file:s.asset.file,id:s.id}];
 if(d.type==='AnimationClip')return [{kind:'clip',file:s.asset.file,sha256:s.asset.sha256,name:d.data.m_Name,stopTime:d.data.m_MuscleClip?.m_StopTime,events:(d.data.m_Events??[]).map(e=>({time:Number(e.time),method:e.functionName,string:e.data,float:Number(e.floatParameter),int:Number(e.intParameter),object:e.objectReferenceParameter,messageOptions:e.messageOptions}))}];
 if(d.type==='BlendTree')return (d.data.m_Childs??[]).flatMap(c=>motions(c.m_Motion,s.asset,visited));
 return [{kind:d.type,file:s.asset.file,id:s.id}];
}
function components(refs,owner){return (refs??[]).map(ref=>source(ref,owner)).filter(Boolean).map(s=>s.asset.byId.get(s.id)).filter(Boolean).map(d=>({id:d.document_id,type:d.type,script:guids.has(d.data.m_Script?.guid)?relative(guids.get(d.data.m_Script.guid)):null,fields:d.data}));}
const fighters=[];
for(const name of roster){
 const prefab=asset(path.join(assets,'Resources/PhotonPrefabs',name+'.prefab'));
 const animator=prefab.documents.find(d=>d.type==='Animator');if(!animator)throw Error('No animator '+name);
 const mapped=source(animator.data.m_Controller,prefab);if(!mapped)throw Error('No controller '+name);const controller=mapped.asset;
 const transitions=controller.documents.filter(d=>['AnimatorStateTransition','AnimatorTransition'].includes(d.type)).map(d=>({id:d.document_id,type:d.type,destination:d.data.m_DstState,destinationStateMachine:d.data.m_DstStateMachine,hasExitTime:d.data.m_HasExitTime,exitTime:d.data.m_ExitTime,duration:d.data.m_TransitionDuration,conditions:d.data.m_Conditions??[]}));
 const states=controller.documents.filter(d=>d.type==='AnimatorState').map(d=>({id:d.document_id,name:d.data.m_Name,speed:d.data.m_Speed,motion:d.data.m_Motion,clips:motions(d.data.m_Motion,controller),behaviours:components(d.data.m_StateMachineBehaviours,controller).map(c=>({id:c.id,script:c.script,fields:c.fields})),transitions:(d.data.m_Transitions??[]).map(r=>transitions.find(t=>t.id===r.fileID)).filter(Boolean)}));
 const prefabScripts=prefab.documents.filter(d=>d.type==='MonoBehaviour'&&guids.has(d.data.m_Script?.guid)).map(d=>({id:d.document_id,gameObject:d.data.m_GameObject.fileID,script:relative(guids.get(d.data.m_Script.guid)),fields:d.data}));
 const entryMachines=controller.documents.filter(d=>d.type==='AnimatorStateMachine').map(d=>({id:d.document_id,name:d.data.m_Name,defaultState:d.data.m_DefaultState,anyStateTransitions:(d.data.m_AnyStateTransitions??[]).map(r=>transitions.find(t=>t.id===r.fileID)).filter(Boolean),entryTransitions:(d.data.m_EntryTransitions??[]).map(r=>transitions.find(t=>t.id===r.fileID)).filter(Boolean),childStateMachines:d.data.m_ChildStateMachines??[],stateMachineTransitions:d.data.m_StateMachineTransitions??[]}));
 fighters.push({name,prefab:prefab.file,prefab_sha256:prefab.sha256,animatorGameObject:animator.data.m_GameObject.fileID,controller:controller.file,controller_sha256:controller.sha256,parameters:controller.documents.find(d=>d.type==='AnimatorController')?.data.m_AnimatorParameters??[],states,stateMachines:entryMachines,prefabScripts});
}
const callbacks=[...new Set(fighters.flatMap(f=>f.states.flatMap(s=>s.clips.flatMap(c=>(c.events??[]).map(e=>e.method))))).values()].filter(Boolean).sort();
const behaviourSources=[...new Set(fighters.flatMap(f=>f.states.flatMap(s=>s.behaviours.map(b=>b.script))).filter(Boolean))].sort();
const codeSources=files.filter(f=>f.endsWith('.cs')).map(file=>{const text=fs.readFileSync(file,'utf8');return {file:relative(file),text,sha256:sha(text)};});
const callbackCandidates=callbacks.map(method=>({method,candidates:codeSources.filter(s=>new RegExp('\\b'+method+'\\s*\\(').test(s.text)).map(s=>({file:s.file,sha256:s.sha256,emptyBodyHeuristic:new RegExp('\\b'+method+'\\s*\\([^)]*\\)\\s*\\{\\s*\\}').test(s.text)}))}));
const report={generatedUtc:new Date().toISOString(),evidenceType:'observed-static-unity-yaml',limitations:['Static controller/clip/event data does not establish actual execution, animation timing fidelity, receiver dispatch, or damage outcomes.','Motion references traverse direct clips and nested blend trees; unknown references remain explicit.','Method source matching and empty-body flags are lexical heuristics, not C# parsing or runtime binding.','AnimationState/Transition lists preserve source conditions and IDs; entry route depends on current runtime state.'],fighters,behaviourSources,callbackCandidates};
const cpu=process.cpuUsage(cpuStarted);
report.measurements={elapsedMs:performance.now()-started,cpuUserMs:cpu.user/1000,cpuSystemMs:cpu.system/1000,peakRssKiB:process.resourceUsage().maxRSS,scope:'single Node process; no child processes; token costs unmeasured'};
fs.writeFileSync('Evidence/roster-moveset-audit.json',JSON.stringify(report,null,2));
console.log(JSON.stringify({fighters:fighters.length,states:fighters.reduce((n,f)=>n+f.states.length,0),callbacks:callbacks.length,measurements:report.measurements}));
