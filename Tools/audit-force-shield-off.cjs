const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const root=path.resolve(__dirname,'..'),sha=p=>crypto.createHash('sha256').update(fs.readFileSync(root+'/'+p)).digest('hex');
(async()=>{
 const {parseUnity}=await import('./unity-audit/audit.mjs');
 const assets=root+'/UnityProject/Assets';
 function walk(p){return fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);}
 const ids=new Map;for(const p of walk(assets+'/Scripts').filter(p=>p.endsWith('.meta'))){const g=fs.readFileSync(p,'utf8').match(/^guid: (\w+)/m)?.[1];if(g)ids.set(g,path.relative(root,p.slice(0,-5)).split(path.sep).join('/'));}
 const reports=[];
 for(const p of fs.readdirSync(assets+'/AnimatorController').filter(p=>p.endsWith('.controller'))){
  const input='UnityProject/Assets/AnimatorController/'+p,parsed=parseUnity(fs.readFileSync(root+'/'+input,'utf8'));if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));const docs=parsed.documents,byId=new Map(docs.map(d=>[d.document_id,d]));
  for(const state of docs.filter(d=>d.type==='AnimatorState'&&['PrisonFree','Freeze'].includes(d.data.m_Name)))reports.push({controller:input,sha256:sha(input),state:state.data.m_Name,behaviours:state.data.m_StateMachineBehaviours.map(r=>({id:r.fileID,script:ids.get(byId.get(r.fileID)?.data.m_Script?.guid),fields:byId.get(r.fileID)?.data})),motion:state.data.m_Motion});
 }
 fs.writeFileSync(root+'/Evidence/force-shield-off-state-bindings.json',JSON.stringify({status:'observed-static',reports},null,2));
 const resolve=(tool,args)=>JSON.parse(cp.execFileSync(process.execPath,[root+'/Tools/'+tool,...args],{encoding:'utf8'}));
 const sources=['ForceShieldOff','SendAnimatorValues','WhipOff'].map(n=>({source:'UnityProject/Assets/Scripts/Assembly-CSharp/'+n+'.cs',source_sha256:sha('UnityProject/Assets/Scripts/Assembly-CSharp/'+n+'.cs'),meta_sha256:sha('UnityProject/Assets/Scripts/Assembly-CSharp/'+n+'.cs.meta'),native:'Evidence/native-disassembly-'+n+'.txt',native_sha256:sha('Evidence/native-disassembly-'+n+'.txt')}));
 fs.writeFileSync(root+'/Evidence/force-shield-off-native-bindings.json',JSON.stringify({status:'native-derived offline implementation; runtime pending',native_sha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',sources,literals:resolve('resolve-native-usage.cjs',['0x28154a0','0x28154a8','0x28154b0','0x28154b8','0x28154c0','0x28154c8','0x28154d0','0x28154d8','0x2814e08']),generic_owners:resolve('resolve-native-generic.cjs',['0x2815468','0x2815470','0x2815478','0x2815480','0x2815488','0x2815490','0x2815498','0x2814830']),limitations:['No Unity or runtime execution.','Native has no Leaf shield owner dispatch.','Photon StopNetBlock RPC remains unreconstructed.']},null,2));
 console.log(JSON.stringify({controller_states:reports.length,source_owners:sources.length}));
})();
