const fs=require('fs'),path=require('path'),crypto=require('crypto');
(async()=>{
 const {parseUnity}=await import('./unity-audit/audit.mjs');
 const root=path.resolve(__dirname,'..');
 const read=p=>{const text=fs.readFileSync(path.join(root,p),'utf8'),parsed=parseUnity(text);if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));return {path:p,sha256:crypto.createHash('sha256').update(text).digest('hex'),docs:parsed.documents};};
 const scene=read('UnityProject/Assets/Scenes/Bending Battle.unity'),byId=new Map(scene.docs.map(d=>[d.document_id,d]));
 const special=scene.docs.find(d=>d.data.m_Script?.guid==='29800c72fda506a0223d9e33053fc3f7');
 const stats=scene.docs.filter(d=>d.data.m_Script?.guid==='631ee25c634b3c78a269d1baeae253f5');
 const button=byId.get('279');
 const ancestors=[];
 let transform=byId.get(button.data.m_Component[0].component.fileID);
 while(transform){const go=byId.get(transform.data.m_GameObject.fileID);ancestors.push({id:go.document_id,name:go.data.m_Name,active:go.data.m_IsActive});transform=byId.get(transform.data.m_Father?.fileID);}
 const result={status:'observed-static',limitations:['No Unity run or original Android runtime observation.','No inferred entitlement or backend access.'],scene:{path:scene.path,sha256:scene.sha256,special_component:special,stats_component_ids:stats.map(d=>d.document_id),special_button:button,button_ancestors:ancestors,button_components:button.data.m_Component.map(r=>byId.get(r.component.fileID))},controllers:[],clips:[]};
 for(const name of ['AirAnimatorController','WaterAnimatorController','EarthAnimatorController','char1_Animation_controller','LightingAnimatorController','ShadowAnimatorController','GrassAnimatorController']){
  const c=read('UnityProject/Assets/AnimatorController/'+name+'.controller'),states=new Map(c.docs.filter(d=>d.type==='AnimatorState').map(d=>[d.document_id,d.data.m_Name]));
  result.controllers.push({path:c.path,sha256:c.sha256,num_specials_default:c.docs.find(d=>d.type==='AnimatorController').data.m_AnimatorParameters.find(p=>p.m_Name==='NumSpecials'),transitions:c.docs.filter(d=>d.type==='AnimatorStateTransition'&&d.data.m_Conditions.some(cond=>cond.m_ConditionEvent==='NumSpecials')).map(d=>({id:d.document_id,destination:states.get(d.data.m_DstState.fileID),conditions:d.data.m_Conditions}))});
 }
 for(const name of ['Awakening','Special_0']){
  const c=read('UnityProject/Assets/AnimationClip/'+name+'.anim');
  result.clips.push({path:c.path,sha256:c.sha256,events:c.docs[0].data.m_Events});
 }
 fs.writeFileSync(path.join(root,'Evidence/special-stats-static-audit.json'),JSON.stringify(result,null,2));
 console.log(JSON.stringify({special_button:button.data.m_Name,active:button.data.m_IsActive,components:button.data.m_Component,controllers:result.controllers.map(c=>({path:c.path,transitions:c.transitions})),clips:result.clips},null,2));
})();
