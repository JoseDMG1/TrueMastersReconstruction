import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {isDeepStrictEqual} from 'node:util';
import {parseDocument} from './node_modules/yaml/dist/index.js';
const root=path.resolve(import.meta.dirname,'../..'),assets=path.join(root,'UnityProject/Assets');
const original=path.join(root,'Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets');
const apply=process.argv.includes('--apply');
const selectedGuidIndex=process.argv.indexOf('--mesh-guid');
const selectedGuid=selectedGuidIndex>=0?process.argv[selectedGuidIndex+1]:null;
if(selectedGuidIndex>=0&&!/^[a-f0-9]{32}$/.test(selectedGuid??''))throw Error('Invalid --mesh-guid');
if(apply&&!selectedGuid)throw Error('Apply requires an explicit consumer-referenced --mesh-guid; default inventory does not authorize broad mutation');
const hash=s=>crypto.createHash('sha256').update(s).digest('hex');
const rel=p=>path.relative(root,p).replaceAll('\\','/');
function walk(dir){return fs.readdirSync(dir,{withFileTypes:true}).flatMap(e=>e.isSymbolicLink()?[]:e.isDirectory()?walk(path.join(dir,e.name)):[path.join(dir,e.name)]);}
const files=walk(assets),guidMap=new Map();
function parse(text,label){const doc=parseDocument(text,{schema:'failsafe',uniqueKeys:true,keepSourceTokens:true});if(doc.errors.length)throw Error(label+': '+doc.errors.map(e=>e.message).join('; '));return doc;}
for(const file of files.filter(f=>f.endsWith('.meta'))){const d=parse(fs.readFileSync(file,'utf8'),file).toJS();if(d.guid){if(guidMap.has(d.guid))throw Error('Duplicate GUID '+d.guid);guidMap.set(d.guid,file.slice(0,-5));}}
function split(raw){const h=[...raw.matchAll(/^--- !u!(\d+) &(-?\d+)(?: stripped)?\r?$/gm)];return h.map((m,i)=>({classId:m[1],id:m[2],offset:m.index+m[0].length,body:raw.slice(m.index+m[0].length,h[i+1]?.index??raw.length)}));}
function field(body,name){const lines=body.split(/\r?\n/),index=lines.findIndex(l=>l.startsWith('  '+name+':'));if(index<0)throw Error('Missing field '+name);let end=index+1;while(end<lines.length&&!/^  [A-Za-z_][^:]*:/.test(lines[end]))end++;return lines.slice(index,end).join('\n');}
function selected(d,names,label){const owner=d.body.trimStart().match(/^(\w+):/)[1];return parse(owner+':\n'+names.map(n=>field(d.body,n)).join('\n'),label).toJS()[owner];}
const sourceCache=new Map();
function source(file){if(!sourceCache.has(file)){const raw=fs.readFileSync(file,'utf8');sourceCache.set(file,{file,hash:hash(raw),docs:split(raw)});}return sourceCache.get(file);}
const consumers=[],unknown=[];
function meshFromRenderer(ref,owner,kind){
 let file=owner.file;
 if(ref.guid&&!/^0+$/.test(ref.guid)){file=guidMap.get(ref.guid);if(!file){unknown.push({reason:'renderer GUID unresolved',ref,file:rel(owner.file)});return null;}}
 const s=source(file),d=s.docs.find(d=>d.id===ref.fileID);
 if(!d){unknown.push({reason:'renderer object unresolved',ref,file:rel(file)});return null;}
 if(kind==='skinned'){if(d.classId!=='137')throw Error('Expected SkinnedMeshRenderer');return selected(d,['m_Mesh'],file).m_Mesh;}
 if(d.classId!=='23')throw Error('Expected MeshRenderer');
 const go=selected(d,['m_GameObject'],file).m_GameObject.fileID;
 const filters=s.docs.filter(d=>d.classId==='33').map(d=>selected(d,['m_GameObject','m_Mesh'],file)).filter(d=>d.m_GameObject.fileID===go);
 if(filters.length!==1){unknown.push({reason:'MeshRenderer must have exactly one same-GameObject MeshFilter',file:rel(file),renderer:ref,count:filters.length});return null;}
 return filters[0].m_Mesh;
}
for(const file of files.filter(f=>/\.(prefab|unity|asset)$/.test(f))){
 const raw=fs.readFileSync(file,'utf8');if(!raw.includes('  ShapeModule:'))continue;
 const owner={file,hash:hash(raw),docs:split(raw)};
 for(const d of owner.docs.filter(d=>d.classId==='198')){
  const shape=selected(d,['m_GameObject','ShapeModule'],file);const module=shape.ShapeModule;
  for(const [property,kind] of [['m_Mesh','mesh'],['m_MeshRenderer','renderer'],['m_SkinnedMeshRenderer','skinned']]){
   const ref=module[property];if(!ref||ref.fileID==='0')continue;
   const mesh=kind==='mesh'?ref:meshFromRenderer(ref,owner,kind);if(!mesh||mesh.fileID==='0')continue;
   consumers.push({file:rel(file),sha256:owner.hash,particleSystem:d.id,gameObject:shape.m_GameObject.fileID,shapeEnabled:module.enabled,shapeType:module.type,property,sourceReference:ref,meshReference:mesh});
  }
 }
 // Retain only documents needed to resolve external renderer PPtrs, not all curves.
}
const targets=new Map();
for(const consumer of consumers){const ref=consumer.meshReference;
 if(!ref.guid||/^0+$/.test(ref.guid)){unknown.push({reason:'local mesh reference unsupported',consumer});continue;}
 if(/^0{16}f0{15}$/.test(ref.guid)){consumer.resolution='Unity built-in resource; not a mutable owned Mesh';continue;}
 const file=guidMap.get(ref.guid);if(!file){unknown.push({reason:'mesh GUID unresolved',consumer});continue;}
 consumer.meshFile=rel(file);
 const key=file+'#'+ref.fileID;if(!targets.has(key))targets.set(key,{file,id:ref.fileID,guid:ref.guid,consumers:[]});targets.get(key).consumers.push(consumer);
}
const changes=[],meshInventory=[];
for(const target of targets.values()){
 const raw=fs.readFileSync(target.file,'utf8'),d=split(raw).find(d=>d.id===target.id&&d.classId==='43');
 if(!d){unknown.push({reason:'referenced object is not a serialized owned Mesh',file:rel(target.file),id:target.id});continue;}
 const doc=parse(d.body,target.file),data=doc.toJS(),node=doc.getIn(['Mesh','m_IsReadable'],true),flag=data.Mesh.m_IsReadable;
 if(!node?.range||!['0','1'].includes(flag))throw Error('Invalid m_IsReadable '+target.file);
 const start=d.offset+node.range[0],end=d.offset+node.range[1];
 if(raw.slice(start,end)!==flag)throw Error('Scalar range mismatch');
 const after=raw.slice(0,start)+'1'+raw.slice(end);
 const edited=parse(split(after).find(x=>x.id===target.id).body,target.file).toJS();
 data.Mesh.m_IsReadable='1';if(!isDeepStrictEqual(data,edited))throw Error('Unexpected semantic change '+target.file);
 const exportFile=path.join(original,path.relative(assets,target.file));
 const originalRaw=fs.existsSync(exportFile)?fs.readFileSync(exportFile,'utf8'):null;
 const originalDoc=originalRaw&&split(originalRaw).find(x=>x.id===target.id&&x.classId==='43');
 const originalData=originalDoc?parse(originalDoc.body,exportFile).toJS():null;
 let onlyReadabilityDiff=null;
 if(originalData){originalData.Mesh.m_IsReadable='1';onlyReadabilityDiff=isDeepStrictEqual(originalData,edited);}
 const item={file:rel(target.file),guid:target.guid,fileID:target.id,name:edited.Mesh.m_Name,beforeReadable:flag,afterReadable:'1',beforeSha256:hash(raw),afterSha256:hash(after),exportFile:originalRaw?rel(exportFile):null,exportSha256:originalRaw?hash(originalRaw):null,differsFromExportOnlyInReadability:onlyReadabilityDiff,consumers:target.consumers};
 meshInventory.push(item);if(flag==='0'&&(!selectedGuid||selectedGuid===target.guid))changes.push({file:target.file,before:raw,after,item});
}
if(selectedGuid&&!meshInventory.some(m=>m.guid===selectedGuid))throw Error('Selected GUID has no examined shape consumer');
const manifest={generatedUtc:new Date().toISOString(),mode:apply?'apply':'plan',scope:'ParticleSystem ShapeModule mesh/MeshRenderer/SkinnedMeshRenderer references in all working .prefab/.unity/.asset files; CPU mesh readability only',filesExamined:files.filter(f=>/\.(prefab|unity|asset)$/.test(f)).length,consumers,meshes:meshInventory,changes:changes.map(c=>c.item),unknowns:unknown,limitations:['Static configured shape references include disabled modules; runtime use depends on particle state.','No geometry, materials, controller, .meta, original exports or unrelated mesh flags are modified.','Owned Mesh assets only; Unity built-in resources remain explicit.','No Unity import/build/player validation performed by this tool.']};
manifest.selectedGuid=selectedGuid;
manifest.unselectedUnreadable=meshInventory.filter(m=>m.beforeReadable==='0'&&selectedGuid&&m.guid!==selectedGuid).map(m=>({guid:m.guid,file:m.file,reason:'Inventory only; not selected for this migration'}));
if(unknown.length){fs.writeFileSync(path.join(root,'Evidence/mesh-shape-readability-plan.json'),JSON.stringify(manifest,null,2));throw Error('Unresolved references: '+unknown.length+'; no asset changes applied');}
if(apply&&changes.length){
 for(const c of consumers){if(hash(fs.readFileSync(path.join(root,c.file)))!==c.sha256)throw Error('Consumer changed before apply '+c.file);}
 const backup=path.join(root,'Evidence/WorkingBackups/mesh-shape-readability-'+new Date().toISOString().replaceAll(':','-'));fs.mkdirSync(backup,{recursive:true});manifest.backupDirectory=rel(backup);
 const written=[];
 try{
  for(const c of changes){if(hash(fs.readFileSync(c.file))!==c.item.beforeSha256)throw Error('Source changed '+c.file);const dest=path.join(backup,path.relative(root,c.file));fs.mkdirSync(path.dirname(dest),{recursive:true});fs.copyFileSync(c.file,dest);}
  fs.writeFileSync(path.join(backup,'manifest.json'),JSON.stringify(manifest,null,2));
  for(const c of changes){if(hash(fs.readFileSync(c.file))!==c.item.beforeSha256)throw Error('Source changed before write '+c.file);fs.writeFileSync(c.file,c.after);written.push(c);if(hash(fs.readFileSync(c.file))!==c.item.afterSha256)throw Error('Write verification failed');}
 }catch(error){for(const c of written.reverse()){if(hash(fs.readFileSync(c.file))===c.item.afterSha256)fs.copyFileSync(path.join(backup,path.relative(root,c.file)),c.file);}throw error;}
}
const outputKind=apply?(changes.length?'applied':'verification'):'plan';
fs.writeFileSync(path.join(root,'Evidence/mesh-shape-readability-'+outputKind+'.json'),JSON.stringify(manifest,null,2));
console.log(JSON.stringify({mode:manifest.mode,files:manifest.filesExamined,consumers:consumers.length,meshes:meshInventory.length,changes:changes.length,unknowns:unknown.length,backup:manifest.backupDirectory}));
