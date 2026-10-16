const fs=require('fs'),cp=require('child_process');
const map=JSON.parse(fs.readFileSync('Evidence/cpp2il-native-address-map.json'));
const fields=new Map(map.fields.filter(f=>f.declaring_type==='Translation').map(f=>[parseInt(f.attributes.Offset,16),f.name]));
const rows=fs.readFileSync('Evidence/native-disassembly-TransladeController.txt','utf8').split(/\r?\n/);
const regs={},assign=[];
function emit(va,reg,offset){if(!fields.has(offset))throw Error('Unknown field '+offset);if(regs[reg]?.kind!=='literal')throw Error('Unresolved register '+reg+' at '+va);assign.push({va,field:fields.get(offset),slot:'0x'+regs[reg].slot.toString(16)});}
for(const row of rows){let m;const va=parseInt(row.match(/^0x([0-9a-f]+)/i)?.[1],16);
 if(m=row.match(/adrp (x\d+), #0x([0-9a-f]+)/))regs[m[1]]={kind:'page',value:parseInt(m[2],16)};
 if(m=row.match(/ldr (x\d+), \[(x\d+), #(0x[0-9a-f]+|\d+)\]/)){const base=regs[m[2]];regs[m[1]]=base?.kind==='page'?{kind:'slot',value:base.value+Number(m[3])}:null;}
 else if(m=row.match(/ldr (x\d+), \[(x\d+)\]/)){const base=regs[m[2]];regs[m[1]]=base?.kind==='slot'?{kind:'literal',slot:base.value}:base?.kind==='page'?{kind:'slot',value:base.value}:null;}
 if(va>=0x12cdf24&&va<=0x12ce6dc){
  if(m=row.match(/str (x\d+), \[x20, #0x([0-9a-f]+)\]/))emit(va,m[1],parseInt(m[2],16));
  if(m=row.match(/stp (x\d+), (x\d+), \[x20, #0x([0-9a-f]+)\]/)){emit(va,m[1],parseInt(m[3],16));emit(va,m[2],parseInt(m[3],16)+8);}
 }
}
const slots=[...new Set(assign.map(a=>a.slot))];
const values=new Map(JSON.parse(cp.execFileSync(process.execPath,['Tools/resolve-native-usage.cjs',...slots],{encoding:'utf8'})).map(s=>[s.slotVA.toLowerCase(),s]));
for(const a of assign){const source=values.get(a.slot);if(source?.kind!==5)throw Error('Not literal '+a.slot);a.value=source.value;}
const unique=new Map(assign.map(a=>[a.field,a]));
console.log(JSON.stringify({fields:fields.size,assignments:assign.length,unique:unique.size,missing:[...fields.values()].filter(f=>!unique.has(f))}));
fs.writeFileSync('Evidence/translation-native-assignments.json',JSON.stringify(assign,null,2));
const csString=s=>JSON.stringify(s).replace(/\\u0000/g,'\\0');
const body='using UnityEngine;\n\n// Literal field assignments recovered from corrected ARM64 0x12CD950..0x12CE6E8.\npublic class TransladeController : MonoBehaviour\n{\n public static TransladeController i;\n public Translation translation;\n private void Awake(){if(i==null)i=this;else if(i!=this){Destroy(this);return;}ChangeToEnglish();}\n private void OnDestroy(){if(i==this)i=null;}\n public void ChangeToEnglish(){\n  translation=new Translation();\n'+assign.map(a=>'  translation.'+a.field+'='+csString(a.value)+';').join('\n')+'\n }\n}\n';
const missing=[...fields.values()].filter(f=>!unique.has(f));
// Corrected native body never writes offset0xC0; retain the new object's null.
if(missing.length!==1||missing[0]!=='Online_Text_WaitingForOtherPlayer')throw Error('Unexpected translation coverage');
fs.writeFileSync('UnityProject/Assets/Scripts/Assembly-CSharp/TransladeController.cs',body);
