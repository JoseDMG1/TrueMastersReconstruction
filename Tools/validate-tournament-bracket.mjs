import fs from 'node:fs';
import crypto from 'node:crypto';
import {parseUnity} from './unity-audit/audit.mjs';
const path=new URL('../UnityProject/Assets/Scenes/Tournament.unity',import.meta.url);
const raw=fs.readFileSync(path,'utf8');
const parsed=parseUnity(raw);if(parsed.errors.length)throw Error(JSON.stringify(parsed.errors));
const docs=new Map(parsed.documents.map(d=>[d.document_id,d]));
const script=guid=>parsed.documents.filter(d=>d.data.m_Script?.guid===guid);
const manager=script('6fc4d90bd6a8a345d30cc9027bfe8ae2')[0].data;
const checkpointComponents=script('8b83cfe6e2971e45cc4bca81d78bcb29');
const allPlayers=new Map(checkpointComponents.map(d=>[d.document_id,d.data]));
const players=manager.players.map(r=>allPlayers.get(r.fileID));
const goComponents=id=>docs.get(id).data.m_Component.map(c=>docs.get(c.component.fileID));
const checkpoint=id=>goComponents(id).find(d=>'cpointOpponent' in d.data).data;
const opponentAt=(player,round)=>checkpoint(player.checkpoints[round].fileID).cpointOpponent.fileID;
const rounds=[];
for(let round=0;round<=6;round++){
 const active=players.filter(p=>p.checkpoints.length>round);
 rounds.push({round,paths:active.map(p=>({player:Number(p.player),checkpoint:p.checkpoints[round].fileID,opponentCheckpoint:opponentAt(p,round)}))});
}
const simulated=[];
// Exercise both coin outcomes for each of three elimination returns.
for(let mask=0;mask<8;mask++){
 const lost=new Set();const positions=new Map(players.map(p=>[p.player,0]));
 let match=0;
 const history=[];
 for(let round=1;round<=6;round++){
  for(const p of players)if(!lost.has(p.player))positions.set(p.player,positions.get(p.player)+1);
  const occupancy=new Map(players.filter(p=>!lost.has(p.player)).map(p=>[p.checkpoints[positions.get(p.player)].fileID,p]));
  if(round%2===1){
   const pairs=[];
   for(const p of players){
    const oppId=opponentAt(p,positions.get(p.player));const other=occupancy.get(oppId);
    if(!other)continue;pairs.push([Number(p.player),Number(other.player)]);
    if(p.player!=='1'&&((mask>>match)&1)===0)lost.add(p.player);else lost.add(other.player);
   }
   history.push({round,pairs,survivors:players.filter(p=>!lost.has(p.player)).map(p=>Number(p.player))});match++;
  }
 }
 if(lost.size!==7||lost.has('1'))throw Error('Invalid winner simulation '+mask);
 simulated.push({mask,history});
}
const report={generatedUtc:new Date().toISOString(),evidenceType:'static-scene-composition-and-model',scene:'UnityProject/Assets/Scenes/Tournament.unity',sha256:crypto.createHash('sha256').update(raw).digest('hex'),playerOrder:players.map(p=>Number(p.player)),rounds,simulated,alertReference:manager.alert,limitations:['No Unity execution; this is a model using recovered scene data and reconstructed control flow.','Win path coins sampled as all equal per round across eight combinations; loseTournament branch not simulated.']};
fs.writeFileSync(new URL('../Evidence/tournament-bracket-static-validation.json',import.meta.url),JSON.stringify(report,null,2));
console.log(JSON.stringify({playerOrder:report.playerOrder,simulations:simulated.length,first:simulated[0].history,alert:manager.alert}));
