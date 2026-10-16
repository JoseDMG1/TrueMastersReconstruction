import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import assert from 'node:assert/strict';
import {parseDocument,isMap,isSeq} from 'yaml';
import {fileURLToPath,pathToFileURL} from 'node:url';

const root=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'../..');
const working=path.join(root,'UnityProject');
const packageRoot='C:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Data/Resources/PackageManager/BuiltInPackages/com.unity.ugui';
const hash=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
const slash=value=>value.split(path.sep).join('/');
// UGUI also supplies the bridge used by TMPHelpURL attributes. Keeping the
// exported stub under this name breaks the real TMP assembly at runtime.
const pluginNames=['UnityEngine.UI','Unity.TextMeshPro','Unity.InternalAPIEngineBridge.004'];
const nativeFields=new Set(['serializedVersion','m_ObjectHideFlags','m_CorrespondingSourceObject','m_PrefabInstance','m_PrefabAsset','m_GameObject','m_Enabled','m_EditorHideFlags','m_Script','m_Name','m_EditorClassIdentifier']);
async function files(dir){const result=[];for(const entry of await fs.readdir(dir,{withFileTypes:true})){const full=path.join(dir,entry.name);if(entry.isSymbolicLink())continue;if(entry.isDirectory())result.push(...await files(full));else if(entry.isFile())result.push(full);}return result.sort();}

export function dllFileId(namespace,name){
  try{return crypto.createHash('md4').update('s\0\0\0'+namespace+name).digest().readInt32LE(0).toString();}
  catch{throw new Error('MD4 needs process-local OpenSSL legacy provider: node --openssl-legacy-provider restore-ui-package.mjs');}
}

export function rewriteScriptRefs(text,mappings){
  const headers=[...text.matchAll(/^--- !u!(\d+) &(-?\d+)(?: stripped)?\r?$/gm)],patches=[],changes=[],before=[];
  if(!headers.length)throw new Error('Referenced asset has no Unity YAML document framing');
  for(let i=0;i<headers.length;i++){
    const header=headers[i],offset=header.index+header[0].length,body=text.slice(offset,headers[i+1]?.index??text.length);
    const doc=parseDocument(body,{schema:'failsafe',uniqueKeys:true,prettyErrors:false,keepSourceTokens:true});
    if(doc.errors.length)throw new Error(`Document ${header[2]}: ${doc.errors.map(error=>error.message).join('; ')}`);
    const value=doc.toJS({maxAliasCount:100});before.push(value);
    const visit=(node,location=[])=>{
      if(isMap(node)){
        const script=node.toJSON();
        if(script?.guid&&mappings.has(script.guid)){
          const kind=location.at(-1);
          if(!['m_Script','script'].includes(kind)||Object.keys(script).sort().join(',')!=='fileID,guid,type')throw new Error(`Unexpected UI DLL reference at ${location.join('.')}`);
          const candidates=mappings.get(script.guid).get(script.fileID)??[];
          if(candidates.length!==1)throw new Error(`Unresolved/ambiguous UI type ${script.guid}/${script.fileID} in document ${header[2]}`);
          const mapping=candidates[0];
          if(!node.range)throw new Error('UI script reference has no source span');
          const replacement={fileID:'11500000',guid:mapping.target_guid,type:'3'};
          const unknownFields=kind==='m_Script'?Object.keys(value[location[0]]).filter(key=>!nativeFields.has(key)&&!mapping.serialized_names.includes(key)):[];
          changes.push({document_id:header[2],reference_path:location,type:mapping.full_name,from:script,to:replacement,unknown_serialized_fields:unknownFields,target_source:mapping.target_source});
          patches.push({start:offset+node.range[0],end:offset+node.range[1],text:`{fileID: 11500000, guid: ${mapping.target_guid}, type: 3}`});
          let parent=value;for(const key of location.slice(0,-1))parent=parent[key];parent[kind]=replacement;
          return;
        }
        for(const pair of node.items)visit(pair.value,[...location,pair.key.value]);
      }else if(isSeq(node))node.items.forEach((item,index)=>visit(item,[...location,index]));
    };
    visit(doc.contents);
  }
  let result=text;
  for(const patch of patches.sort((a,b)=>b.start-a.start))result=result.slice(0,patch.start)+patch.text+result.slice(patch.end);
  const afterHeaders=[...result.matchAll(/^--- !u!(\d+) &(-?\d+)(?: stripped)?\r?$/gm)];
  assert.equal(afterHeaders.length,headers.length);
  for(let i=0;i<afterHeaders.length;i++){
    assert.equal(afterHeaders[i][0],headers[i][0]);
    const offset=afterHeaders[i].index+afterHeaders[i][0].length;
    const after=parseDocument(result.slice(offset,afterHeaders[i+1]?.index??result.length),{schema:'failsafe',uniqueKeys:true}).toJS({maxAliasCount:100});
    assert.deepStrictEqual(after,before[i],`Unintended semantic change in document ${headers[i][2]}`);
  }
  return {text:result,changes};
}

export async function plan(){
  const schemas=JSON.parse(await fs.readFile(path.join(root,'Evidence','ui-type-schema.json'),'utf8'));
  const official=schemas.filter(item=>item.origin==='official'),sources=(await files(path.join(packageRoot,'Runtime'))).filter(file=>file.endsWith('.cs'));
  const packageJson=JSON.parse(await fs.readFile(path.join(packageRoot,'package.json'),'utf8'));
  if(packageJson.version!=='2.6.0')throw new Error('Unexpected bundled ugui version');
  const mappings=new Map(),typeMappings=[],plugins=[];
  for(const name of pluginNames){
    const dll=path.join(working,'Assets','Plugins',name+'.dll'),meta=dll+'.meta';
    const metaBytes=await fs.readFile(meta),guid=parseDocument(metaBytes.toString('utf8'),{schema:'failsafe'}).get('guid');
    mappings.set(guid,new Map());
    plugins.push({dll,meta,guid,dll_sha256:hash(await fs.readFile(dll)),meta_sha256:hash(metaBytes)});
    for(const type of schemas.filter(item=>item.origin==='placeholder'&&item.assembly===name)){
      const destination=official.filter(item=>item.assembly===name&&item.full_name===type.full_name);
      if(destination.length!==1)continue;
      const candidates=sources.filter(file=>path.basename(file)===type.name+'.cs');
      if(candidates.length!==1)continue;
      const target=candidates[0],targetMeta=await fs.readFile(target+'.meta');
      const targetGuid=parseDocument(targetMeta.toString('utf8'),{schema:'failsafe'}).get('guid');
      const entry={full_name:type.full_name,from_guid:guid,from_file_id:dllFileId(type.namespace,type.name),target_guid:targetGuid,target_source:slash(path.relative(packageRoot,target)),serialized_names:[...new Set(destination[0].serialized_fields.flatMap(field=>[field.name,...field.aliases]))]};
      const byId=mappings.get(guid);byId.set(entry.from_file_id,[...(byId.get(entry.from_file_id)??[]),entry]);typeMappings.push(entry);
    }
  }
  const assets=[];
  for(const file of (await files(path.join(working,'Assets'))).filter(file=>/\.(unity|prefab|asset|anim|controller|overrideController)$/.test(file))){
    const bytes=await fs.readFile(file),text=bytes.toString('utf8');
    if(!plugins.some(plugin=>text.includes(plugin.guid)))continue;
    const rewritten=rewriteScriptRefs(text,mappings);
    if(!rewritten.changes.length)continue;
    assets.push({file,relative:slash(path.relative(working,file)),before_sha256:hash(bytes),after_sha256:hash(rewritten.text),changes:rewritten.changes});
  }
  const manifestFile=path.join(working,'Packages','manifest.json'),manifestBytes=await fs.readFile(manifestFile),manifest=JSON.parse(manifestBytes.toString('utf8'));
  if(manifest.dependencies['com.unity.ugui']&&manifest.dependencies['com.unity.ugui']!=='2.6.0')throw new Error('Conflicting existing ugui dependency');
  const report={version:1,working,package:{name:packageJson.name,version:packageJson.version,bundled_path:packageRoot,source:'Official package bundled with installed Unity6000.6.5f1; editor catalog minimum/version2.6.0'},
    manifest:{file:manifestFile,before_sha256:hash(manifestBytes)},plugins,type_mappings:typeMappings,assets,
    refs_to_update:assets.reduce((sum,asset)=>sum+asset.changes.length,0),
    unknown_fields:assets.flatMap(asset=>asset.changes.filter(change=>change.unknown_serialized_fields.length).map(change=>({file:asset.relative,...change}))),
    preservation:'Every parsed YAML document semantically identical except intended UI m_Script and animation script PPtr mapping; original textual fields retained by source-span editing.',
    limitations:['Static compatibility check; Unity compile/import/runtime validation still required.','SDKs outside official UI/TMP remain placeholders.','Target script GUIDs are read from official package source meta, old DLL fileIDs derived from exact Mono.Cecil namespace/type names using Unity MD4 convention.']};
  await fs.writeFile(path.join(root,'Evidence','ui-package-migration-plan.json'),JSON.stringify(report,null,2)+'\n');
  return report;
}

async function apply(){
  const report=await plan();
  if(report.unknown_fields.length)throw new Error(`Unresolved serialized UI fields (${report.unknown_fields.length} components): inspect plan before applying`);
  const stamp=new Date().toISOString().replace(/[:.]/g,'-'),backupRoot=path.join(root,'Evidence','WorkingBackups',`ui-package-${stamp}`);
  await fs.mkdir(backupRoot,{recursive:true});
  const mappings=new Map();
  for(const mapping of report.type_mappings){if(!mappings.has(mapping.from_guid))mappings.set(mapping.from_guid,new Map());const byId=mappings.get(mapping.from_guid);byId.set(mapping.from_file_id,[...(byId.get(mapping.from_file_id)??[]),mapping]);}
  // Prepare and validate every file before the first working-project mutation.
  const prepared=[];
  for(const asset of report.assets){
    const bytes=await fs.readFile(asset.file);if(hash(bytes)!==asset.before_sha256)throw new Error(`Changed during migration: ${asset.file}`);
    const rewritten=rewriteScriptRefs(bytes.toString('utf8'),mappings);
    if(hash(rewritten.text)!==asset.after_sha256)throw new Error('Non-deterministic planned UI patch');
    const backup=path.join(backupRoot,asset.relative);await fs.mkdir(path.dirname(backup),{recursive:true});await fs.writeFile(backup,bytes);
    prepared.push({file:asset.file,text:rewritten.text,before:bytes});
  }
  const manifestBytes=await fs.readFile(report.manifest.file);if(hash(manifestBytes)!==report.manifest.before_sha256)throw new Error('Manifest changed during migration');
  await fs.mkdir(path.join(backupRoot,'Packages'),{recursive:true});await fs.writeFile(path.join(backupRoot,'Packages','manifest.json'),manifestBytes);
  const pluginBackups=[];
  for(const plugin of report.plugins){
    await fs.mkdir(path.join(backupRoot,'Assets','Plugins'),{recursive:true});
    for(const [file,expected]of [[plugin.dll,plugin.dll_sha256],[plugin.meta,plugin.meta_sha256]]){const bytes=await fs.readFile(file);if(hash(bytes)!==expected)throw new Error(`Plugin changed during migration: ${file}`);await fs.writeFile(path.join(backupRoot,'Assets','Plugins',path.basename(file)),bytes);pluginBackups.push({file,before:bytes});}
  }
  await fs.writeFile(path.join(backupRoot,'migration-plan.json'),JSON.stringify(report,null,2));
  const touched=[];
  try{
    for(const entry of prepared){touched.push(entry);await fs.writeFile(entry.file,entry.text);}
    const manifest=JSON.parse(manifestBytes.toString('utf8'));manifest.dependencies['com.unity.ugui']='2.6.0';
    touched.push({file:report.manifest.file,before:manifestBytes});await fs.writeFile(report.manifest.file,JSON.stringify(manifest,null,2)+'\n');
    // Exact owned files only, all backed up above. Immutable native exports untouched.
    for(const entry of pluginBackups){touched.push(entry);await fs.unlink(entry.file);}
  }catch(error){
    const failed=[];
    for(const entry of touched.reverse()){try{await fs.writeFile(entry.file,entry.before);}catch(rollbackError){failed.push({file:entry.file,error:rollbackError.message});}}
    await fs.writeFile(path.join(backupRoot,'failure.json'),JSON.stringify({error:error.message,rollback_failures:failed},null,2));
    throw new Error(`UI migration failed: ${error.message}; rollback failures: ${failed.length}; backups: ${backupRoot}`);
  }
  const result={status:'applied',backup:backupRoot,changed_assets:prepared.length,script_references:report.refs_to_update,package:'com.unity.ugui@2.6.0',validation:'YAML semantic equality passed; awaiting root Unity batch import/runtime verification'};
  await fs.writeFile(path.join(root,'Evidence','ui-package-migration-applied.json'),JSON.stringify(result,null,2)+'\n');
  return result;
}

if(process.argv[1]&&import.meta.url===pathToFileURL(path.resolve(process.argv[1])).href){
  (process.argv.includes('--apply')?apply():plan()).then(report=>console.log(JSON.stringify(report.status?report:{assets:report.assets.length,refs:report.refs_to_update,unknown_components:report.unknown_fields.length,types:report.type_mappings.length,package:report.package},null,2))).catch(error=>{console.error(error);process.exitCode=1;});
}
