const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..'),assets=path.join(root,'UnityProject/Assets');
function walk(p){return fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);}
const files=walk(assets),guids=new Map;
for(const p of files.filter(p=>p.endsWith('.meta'))){const m=fs.readFileSync(p,'utf8').match(/^guid: (\w+)/m);if(m)guids.set(m[1],p.slice(0,-5));}
const relative=p=>path.relative(root,p).replaceAll('\\','/');
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const result=[];
for(const name of ['Sora','Amon']){
 const prefab=path.join(assets,'Resources/photonprefabs/'+name+'.prefab'),body=fs.readFileSync(prefab,'utf8');
 const guid=body.match(/m_Controller: \{fileID: 9100000, guid: (\w+)/)[1],controller=guids.get(guid),text=fs.readFileSync(controller,'utf8');
 const behaviours=[...new Set([...text.matchAll(/m_Script: \{fileID: 11500000, guid: (\w+)/g)].map(m=>guids.get(m[1])).filter(Boolean))].map(relative);
 const states=[];
 for(const block of text.split(/^--- /m).filter(s=>s.startsWith('!u!1102 '))){const n=block.match(/^  m_Name: (.+)$/m)?.[1],g=block.match(/m_Motion: \{fileID: 7400000, guid: (\w+)/)?.[1],clip=guids.get(g);if(!clip)continue;
 const c=fs.readFileSync(clip,'utf8'),eventSection=c.slice(c.lastIndexOf('  m_Events:'));
 states.push({state:n,clip:relative(clip),clip_sha256:hash(clip),events:[...eventSection.matchAll(/- time: ([^\r\n]+)\r?\n    functionName: ([^\r\n]+)/g)].map(m=>({time:Number(m[1]),method:m[2]}))});}
 result.push({name,prefab:relative(prefab),prefab_sha256:hash(prefab),controller:relative(controller),controller_sha256:hash(controller),behaviours,states});
}
fs.writeFileSync(path.join(root,'Evidence/fighter-animation-audit.json'),JSON.stringify({status:'observed-static-yaml',limitations:['Clip events do not establish execution or runtime receiver binding.','Only direct controller clip references examined; blend-tree clips are not traversed.'],fighters:result},null,2));
console.log(JSON.stringify(result.map(r=>({name:r.name,controller:r.controller,behaviours:r.behaviours,attackStates:r.states.filter(s=>s.state.includes('Attack'))})),null,2));
