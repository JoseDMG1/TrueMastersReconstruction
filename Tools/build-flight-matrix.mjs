import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const sha=f=>crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const dependencies=JSON.parse(fs.readFileSync('Evidence/ability-dependencies.json','utf8'));
const selected=new Map();
for(const fighter of dependencies.fighters) for(const state of fighter.states) for(const prefab of state.dependencies) {
    const follow=prefab.components.find(c=>c.enabled==='1' && c.script.endsWith('/Follow.cs'));
    const light=prefab.components.find(c=>c.enabled==='1' && c.script.endsWith('/MoveLight.cs'));
    if(!follow && !light) continue;
    const existing=selected.get(prefab.file);
    if(existing) {existing.fighters.add(fighter.name);continue;}
    const root=(follow??light).gameObject;
    const docs=parseUnity(fs.readFileSync(prefab.file,'utf8')).documents;
    const rootId=docs.find(d=>d.type==='Transform'&&d.data.m_Father?.fileID==='0')?.data.m_GameObject?.fileID;
    let dormantFollow=false;
    if(follow&&root!==rootId) {
        let id=root;
        while(id){const obj=docs.find(d=>d.type==='GameObject'&&d.document_id===id);if(obj?.data.m_IsActive==='0')dormantFollow=true;
            const t=docs.find(d=>d.type==='Transform'&&d.data.m_GameObject?.fileID===id);const parent=docs.find(d=>d.document_id===t?.data.m_Father?.fileID);id=parent?.data.m_GameObject?.fileID;}
        if(!dormantFollow)throw Error('Active nested Follow needs a separate binding contract: '+prefab.file);
    }
    const forward=prefab.components.find(c=>c.enabled==='1' && c.gameObject===root && c.script.endsWith('/GoForward.cs'));
    const delayed=prefab.components.filter(c=>c.enabled==='1' && c.gameObject===root && /\/(MoveLight|MoveWater|MoveAttack|GoForward)\.cs$/.test(c.script));
    const rotations=prefab.components.filter(c=>c.enabled==='1'&&c.gameObject===root&&c.script.endsWith('/StartRotation.cs'));
    selected.set(prefab.file,{file:prefab.file,sha256:sha(prefab.file),resource:prefab.file.split('/Resources/')[1].replace(/\.prefab$/,''),
        name:path.basename(prefab.file,'.prefab'),follow:!!follow,dormantFollow,light:!!light,forward:!!forward,
        followSpeed:Number(follow?.fields.speed??0),startFaster:follow?.fields.startFaster==='1',startSpeed:Number(follow?.fields.startSpeed??1),startTime:Number(follow?.fields.startTime??0),
        delay:Math.max(0,...delayed.map(c=>Number(c.fields.timeToStart??c.fields.delay??0))),lightSpeed:Number(light?.fields.speed??0),forwardSpeed:Number(forward?.fields.speed??0),
        rotationDelay:Math.max(0,...rotations.map(c=>Number(c.fields.delay??0))),
        edge:prefab.components.some(c=>c.enabled==='1'&&c.gameObject===root&&c.script.endsWith('/DestroyOnEdge.cs')),fighters:new Set([fighter.name])});
}
const profiles=[...selected.values()].map(p=>({...p,fighters:[...p.fighters]}));
const walk=p=>fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);
const resources=new Map();
for(const file of walk('UnityProject/Assets/Resources').filter(f=>f.endsWith('.prefab.meta'))) {
    const guid=fs.readFileSync(file,'utf8').match(/^guid: (\w+)/m)?.[1];
    if(guid)resources.set(guid,file.slice(0,-5).replaceAll('\\','/'));
}
const audit=JSON.parse(fs.readFileSync('Evidence/awakened-attack-event-audit.json','utf8'));
const jumps=audit.fighters.flatMap(f=>f.callbacks.map(c=>{
    const file=resources.get(c.receiver.guid);if(!file)throw Error(c.receiver.guid);
    return {fighter:f.fighter,state:c.state,clip:c.clipFile,eventTime:c.time,resource:file.split('/Resources/')[1].replace(/\.prefab$/,''),name:path.basename(file,'.prefab'),file,sha256:sha(file)};
}));
const inventory={generatedUtc:new Date().toISOString(),scope:'Static authored profile and awakening-event inventory, not natural runtime coverage',
    sources:['Evidence/ability-dependencies.json','Evidence/awakened-attack-event-audit.json'].map(file=>({file,sha256:sha(file)})),profiles,jumps};
fs.writeFileSync('Evidence/flight-matrix-inventory.json',JSON.stringify(inventory,null,2)+'\n');
fs.writeFileSync('Tools/unity-validation/FlightMatrixProfiles.json',JSON.stringify({profiles,jumps},null,2)+'\n');
console.log(JSON.stringify({profiles:profiles.length,follow:profiles.filter(p=>p.follow).length,straightLight:profiles.filter(p=>!p.follow&&p.light).length,jumps}));
