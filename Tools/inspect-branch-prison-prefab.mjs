import fs from 'node:fs';import {parseUnity} from './unity-audit/audit.mjs';
const names=['branchPrisonController','BranchPrisonAnim','PathController','BonePath','RefHitCharacter','SimpleMove','DestroyByTime'];
const types=new Map(names.map(n=>[fs.readFileSync('UnityProject/Assets/Scripts/Assembly-CSharp/'+n+'.cs.meta','utf8').match(/^guid: (.+)$/m)[1].trim(),n]));
const file='UnityProject/Assets/Resources/PhotonPrefabs/BranchPrisonATK.prefab';
const p=parseUnity(fs.readFileSync(file,'utf8'));if(p.errors.length)throw Error(JSON.stringify(p.errors));
for(const d of p.documents){if(types.has(d.data.m_Script?.guid))console.log(JSON.stringify({type:types.get(d.data.m_Script.guid),id:d.document_id,fields:d.data}));}
