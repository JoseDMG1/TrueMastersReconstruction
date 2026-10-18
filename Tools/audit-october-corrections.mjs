import fs from 'node:fs';
import assert from 'node:assert/strict';
import crypto from 'node:crypto';
import { parseUnity } from './unity-audit/audit.mjs';
const root = new URL('../', import.meta.url);
const read = p => fs.readFileSync(new URL(p, root), 'utf8');
const checks = [];
function asset(p) {
  const parsed = parseUnity(read(p));
  assert.deepEqual(parsed.errors, [], p);
  checks.push({ file:p, yamlDocuments:parsed.documents.length, sha256:crypto.createHash('sha256').update(read(p)).digest('hex') });
  return parsed.documents;
}
const base='UnityProject/Assets/';
const controller=asset(base+'AnimatorController/LightingAnimatorController.controller');
const right=controller.find(d=>d.type==='AnimatorState'&&d.data.m_Name==='JumpR');
const left=controller.find(d=>d.type==='AnimatorState'&&d.data.m_Name==='JumpL');
assert.equal(right.data.m_Mirror,'1'); assert.equal(left.data.m_Mirror,'0');
const controllerGuid=read(base+'AnimatorController/LightingAnimatorController.controller.meta').match(/guid: (\w+)/)[1];
for(const fighter of ['Dante','Agata']) {
  const docs=asset(base+'Resources/photonprefabs/'+fighter+'.prefab');
  assert(docs.some(d=>d.type==='Animator'&&d.data.m_Controller?.guid===controllerGuid),fighter+' controller');
}
for(const [state,clip,sign] of [[right,'JumpR_4',1],[left,'JumpL_4',-1]]) {
  const clipGuid=read(base+'AnimationClip/'+clip+'.anim.meta').match(/guid: (\w+)/)[1];
  assert.equal(state.data.m_Motion.guid,clipGuid);
  const text=read(base+'AnimationClip/'+clip+'.anim');
  const events=text.slice(text.lastIndexOf('  m_Events:'));
  assert.match(events,new RegExp('functionName: goHorizontal[\\s\\S]*?floatParameter: '+(sign<0?'-':'')+'0\\.37'));
}
const movementGuid=read(base+'Scripts/Assembly-CSharp/MoveAttack.cs.meta').match(/guid: (\w+)/)[1];
const followGuid=read(base+'Scripts/Assembly-CSharp/Follow.cs.meta').match(/guid: (\w+)/)[1];
for(const name of ['AirAtkL2','AirAtkR2']) {
  const docs=asset(base+'Resources/photonprefabs/'+name+'.prefab');
  const move=docs.find(d=>d.data.m_Script?.guid===movementGuid).data;
  const follow=docs.find(d=>d.data.m_Script?.guid===followGuid).data;
  assert.equal(follow.m_Enabled,'0'); assert.equal(move.aimAtTarget,'0');
  assert.deepEqual(move.fixedLocalDirection,{x:'0',y:'0',z:'1'});
  assert.equal(move.timeToStart,'0.75');assert.equal(move.speed,'0.55');
  const guid=read(base+'Resources/photonprefabs/'+name+'.prefab.meta').match(/guid: (\w+)/)[1];
  // Confirm real air animation still invokes the factory that sets +/-90 degree launch yaw.
  const air=asset(base+'AnimatorController/AirAnimatorController.controller');
  const state=air.find(d=>d.type==='AnimatorState'&&d.data.m_Name===(name==='AirAtkL2'?'LeftAttack2':'RightAttack2'));
  const metas=fs.readdirSync(new URL(base+'AnimationClip/',root)).filter(f=>f.endsWith('.meta'));
  const meta=metas.find(f=>read(base+'AnimationClip/'+f).includes('guid: '+state.data.m_Motion.guid));
  const clip=read(base+'AnimationClip/'+meta.slice(0,-5));
  const events=clip.slice(clip.lastIndexOf('  m_Events:'));
  assert.match(events,new RegExp('functionName: CreateKickAttack[\\s\\S]*?guid: '+guid));
}
for(const scene of ['Bending Battle','StartMenu','Tournament']) {
  const docs=asset(base+'Scenes/'+scene+'.unity');
  const labels=docs.filter(d=>d.data.m_Text==='SELECT CHARACTER'||d.data.m_text==='SELECT CHARACTER');
  assert(labels.length>0);
  assert(!docs.some(d=>/^play again\s*$/i.test(d.data.m_Text??d.data.m_text??'')));
  for(const label of labels) {
    if(label.data.m_FontData) assert.equal(label.data.m_FontData.m_BestFit,'1');
    else assert.equal(label.data.m_enableAutoSizing,'1');
  }
  checks.push({scene,selectionLabels:labels.length});
}
// Preserve every authored transform scale in the animal resources; correction is in growth code.
for(const animal of ['Bat','Bird','Wolf','Spider','Snake','Shark']) {
  const relative='Resources/photonprefabs/Shadow'+animal+'Atk.prefab';
  const current=asset(base+relative).filter(d=>d.type==='Transform');
  const originalPath='Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets/'+relative;
  if(fs.existsSync(new URL(originalPath,root))) {
    const original=new Map(asset(originalPath).filter(d=>d.type==='Transform').map(d=>[d.document_id,d.data.m_LocalScale]));
    for(const transform of current) assert.deepEqual(transform.data.m_LocalScale,original.get(transform.document_id),animal+' authored scale '+transform.document_id);
  }
}
const report={generatedUtc:new Date().toISOString(),result:'passed',scope:'Serialized asset syntax, GUID/controller/event routing, authored scale preservation and selection-label sizing. Does not execute Unity or establish visual/runtime acceptance.',checks};
fs.writeFileSync(new URL('Evidence/october-corrections-assets.json',root),JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({result:report.result,checks:checks.length,scope:report.scope},null,2));
