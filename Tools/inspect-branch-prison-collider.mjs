import fs from 'node:fs';import {parseUnity} from './unity-audit/audit.mjs';
const p=parseUnity(fs.readFileSync('UnityProject/Assets/Resources/PhotonPrefabs/BranchPrisonATK.prefab','utf8'));
console.log(JSON.stringify(p.documents.filter(d=>/Collider/.test(d.type)||d.data.Type==='Atk'||d.data.Type==='Attack'||d.data.effect==='branchPrison'),null,2));
