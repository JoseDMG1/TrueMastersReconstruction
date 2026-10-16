const fs=require('fs'),path=require('path'),crypto=require('crypto');
(async()=>{
const {parseUnity}=await import('./unity-audit/audit.mjs');const root=path.resolve(__dirname,'..'),assets=root+'/UnityProject/Assets';
function walk(p){return fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);}
const guids=new Map;for(const directory of ['Scripts','AnimationClip'])for(const p of walk(assets+'/'+directory).filter(p=>p.endsWith('.meta'))){const g=fs.readFileSync(p,'utf8').match(/^guid: (\w+)/m)?.[1];if(g)guids.set(g,path.relative(root,p.slice(0,-5)).split(path.sep).join('/'));}
const reports=[];
for(const name of ['AirAnimatorController','WaterAnimatorController','EarthAnimatorController','char1_Animation_controller','LightingAnimatorController','ShadowAnimatorController','GrassAnimatorController']){
const input='UnityProject/Assets/AnimatorController/'+name+'.controller',text=fs.readFileSync(root+'/'+input,'utf8'),parsed=parseUnity(text);if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));const docs=parsed.documents,byId=new Map(docs.map(d=>[d.document_id,d]));
const transition=t=>({id:t.document_id,conditions:t.data.m_Conditions,destination:byId.get(t.data.m_DstState?.fileID)?.data.m_Name||null,destination_machine:byId.get(t.data.m_DstStateMachine?.fileID)?.data.m_Name||null,exit:t.data.m_IsExit,has_exit_time:t.data.m_HasExitTime,exit_time:t.data.m_ExitTime,duration:t.data.m_TransitionDuration});
const states=docs.filter(d=>d.type==='AnimatorState'&&['Block','BlockHit','hitBlocking'].includes(d.data.m_Name)).map(d=>{
 const clip=guids.get(d.data.m_Motion?.guid),clipData=clip?parseUnity(fs.readFileSync(root+'/'+clip,'utf8')).documents[0].data:null;
 return {id:d.document_id,name:d.data.m_Name,behaviours:d.data.m_StateMachineBehaviours.map(r=>({id:r.fileID,script:guids.get(byId.get(r.fileID)?.data.m_Script?.guid),fields:byId.get(r.fileID)?.data})),clip,events:clipData?.m_Events,outgoing:d.data.m_Transitions.map(r=>transition(byId.get(r.fileID))),machines:docs.filter(m=>m.type==='AnimatorStateMachine'&&m.data.m_DefaultState?.fileID===d.document_id).map(m=>({name:m.data.m_Name,id:m.document_id,incoming:docs.filter(t=>t.type==='AnimatorStateTransition'&&t.data.m_DstStateMachine?.fileID===m.document_id).map(transition)}))};
});reports.push({input,sha256:crypto.createHash('sha256').update(text).digest('hex'),states});
}
fs.writeFileSync(root+'/Evidence/defense-controller-static-audit.json',JSON.stringify({status:'observed-static',limitations:['No runtime state or animation callback execution established.','Direct clips only; not a full animation blend-tree audit.'],reports},null,2));
console.log(JSON.stringify(reports.map(r=>({input:r.input,states:r.states.map(s=>({name:s.name,behaviours:s.behaviours.map(b=>b.script?.split('/').pop()),events:s.events?.map(e=>({time:e.time,method:e.functionName})),machines:s.machines,outgoing:s.outgoing}))})),null,2));
})();
