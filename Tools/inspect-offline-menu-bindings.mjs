import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
import {parseDocument} from './unity-audit/node_modules/yaml/dist/index.js';

const methods=new Set(['OpenSettings','CloseSettings','Unlock','OpenStageSelector','nextStage','previousStage','SelectStage','CloseStageSelector']);
const ugui='C:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Data/Resources/PackageManager/BuiltInPackages/com.unity.ugui';
const componentSources=[['UnityEngine.UI.Button','Runtime/UGUI/UI/Core/Button.cs.meta'],['UnityEngine.EventSystems.EventTrigger','Runtime/UGUI/EventSystem/EventTrigger.cs.meta']];
const componentByGuid=new Map(componentSources.map(([type,file])=>{
 const source=path.join(ugui,file),raw=fs.readFileSync(source,'utf8');
 const meta=parseDocument(raw,{schema:'failsafe',uniqueKeys:true});
 if(meta.errors.length)throw Error('Invalid component metadata '+source);
 return [meta.toJS().guid,{type,source}];
}));
const reports=[];
for(const scene of ['Character Selection','Bending Battle']){
 const file='UnityProject/Assets/Scenes/'+scene+'.unity';
 const raw=fs.readFileSync(file,'utf8');
 const {documents,errors}=parseUnity(raw);
 if(errors.length)throw Error(JSON.stringify(errors));
 const docs=new Map(documents.map(d=>[d.document_id,d]));
 const transformByGO=new Map(documents.filter(d=>['Transform','RectTransform'].includes(d.type)).map(d=>[d.data.m_GameObject.fileID,d]));
 function objectPath(id){
  const go=docs.get(id),t=transformByGO.get(id),parent=t&&docs.get(t.data.m_Father.fileID);
  return (parent?objectPath(parent.data.m_GameObject.fileID)+'/':'')+go.data.m_Name;
 }
 for(const d of documents){
  const identity=componentByGuid.get(d.data.m_Script?.guid);
  if(!identity)continue;
  let bindings;
  if(identity.type==='UnityEngine.UI.Button'){
   if(!d.data.m_OnClick)throw Error('Button missing m_OnClick '+d.document_id);
   bindings=[{kind:'Button.onClick',property:'m_OnClick',eventID:null,calls:d.data.m_OnClick.m_PersistentCalls?.m_Calls??[]}];
  }else{
   if(!Array.isArray(d.data.m_Delegates))throw Error('EventTrigger missing m_Delegates '+d.document_id);
   bindings=d.data.m_Delegates.map((entry,index)=>({kind:'EventTrigger.entry',property:'m_Delegates['+index+'].callback',eventID:Number(entry.eventID),calls:entry.callback?.m_PersistentCalls?.m_Calls??[]}));
  }
  for(const binding of bindings){
   const calls=binding.calls.filter(c=>methods.has(c.m_MethodName));
   if(!calls.length)continue;
   const item={scene,file,sha256:crypto.createHash('sha256').update(raw).digest('hex'),component:d.document_id,path:objectPath(d.data.m_GameObject.fileID),componentType:identity.type,scriptGuid:d.data.m_Script.guid,componentIdentitySource:identity.source,bindingKind:binding.kind,property:binding.property,eventID:binding.eventID,eventName:binding.eventID===4?'PointerClick':null,calls:calls.map(c=>({method:c.m_MethodName,target:c.m_Target.fileID,targetGO:objectPath(docs.get(c.m_Target.fileID).data.m_GameObject.fileID),callState:c.m_CallState}))};
   reports.push(item);console.log(JSON.stringify(item));
  }
 }
}
fs.writeFileSync('Evidence/offline-menu-binding-inventory.json',JSON.stringify({generatedUtc:new Date().toISOString(),evidenceType:'static-scene-bindings',bindings:reports},null,2));
