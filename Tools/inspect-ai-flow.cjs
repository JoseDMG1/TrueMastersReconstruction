// Reading aid only: corrected native source is authoritative. No code generation.
const fs=require('fs'),cp=require('child_process');
const base=__dirname+'/..';
const map=JSON.parse(fs.readFileSync(base+'/Evidence/cpp2il-native-address-map.json'));
const types=JSON.parse(fs.readFileSync(base+'/Evidence/assembly-csharp-metadata.json')).types;
const names=new Map(map.entries.map(e=>[parseInt(e.native_address,16),e.declaring_type+'.'+e.method_signature]));
const owner=process.argv[3]||'AI';
const text=fs.readFileSync(base+'/Evidence/native-disassembly-'+owner+'.txt','utf8');
const binary=fs.readFileSync(base+'/Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so');
const name=process.argv[2];
const section=text.split(/(?=^Method:)/m).find(s=>s.startsWith('Method: ')&&s.slice(8).includes(' '+name+'('));
if(!section)throw Error('No method');
const rows=section.trim().split(/\r?\n/),regs={},slots=new Set;
for(const row of rows){let m;if(m=row.match(/adrp (x\d+), #0x([0-9a-f]+)/))regs[m[1]]=parseInt(m[2],16);if(m=row.match(/ldr x\d+, \[(x\d+), #0x([0-9a-f]+)\]/)){let slot=regs[m[1]]+parseInt(m[2],16);if(slot>=0x2814000&&slot<0x281a000)slots.add('0x'+slot.toString(16));}}
const usages=JSON.parse(cp.execFileSync(process.execPath,[__dirname+'/resolve-native-usage.cjs',...slots],{encoding:'utf8'}));
const usage=new Map(usages.map(u=>[parseInt(u.slotVA,16),u.value||(u.kind===3?types.flatMap(t=>t.methods).find(m=>m.index===u.index)?.name:null)]));
Object.keys(regs).forEach(k=>delete regs[k]);
const animatorContracts=JSON.parse(fs.readFileSync(__dirname+'/animator-native-call-contracts.json','utf8').replace(/^\uFEFF/,''));
const known={...Object.fromEntries(animatorContracts.contracts.map(c=>[parseInt(c.address,16).toString(16),c.signature])),'2362760':'State.IsName','2363e50':'Animator.StateInfo','2362e44':'Animator.GetBool','236291c':'Animator.GetFloat','23631a8':'Animator.GetInteger','239d570':'Random.Range','2362914':'State.normalizedTime','2363ed0':'Animator.IsInTransition'};
for(let row of rows){let m;if(m=row.match(/adrp (x\d+), #0x([0-9a-f]+)/))regs[m[1]]=parseInt(m[2],16);
if(m=row.match(/ldr x\d+, \[(x\d+), #0x([0-9a-f]+)\]/)){let slot=regs[m[1]]+parseInt(m[2],16);if(usage.has(slot))row+=' // '+JSON.stringify(usage.get(slot));}
if(m=row.match(/ldr ([sd])\d+, \[(x\d+), #0x([0-9a-f]+)\]/)){let va=regs[m[2]]+parseInt(m[3],16),s=map.segments.find(s=>va>=s.virtualAddress&&va<s.virtualAddress+s.fileSize);if(s&&va<0x1000000){let o=va-s.virtualAddress+s.fileOffset;row+=' // '+(m[1]==='s'?binary.readFloatLE(o):binary.readDoubleLE(o));}}
if(m=row.match(/(?:bl|b) #0x([0-9a-f]+)/)){let n=names.get(parseInt(m[1],16))||known[m[1]];if(n)row+=' // '+n;}
// Suppress only stack/call setup; retain comparison and branch addresses.
if(/ (?:stp|ldp|ldur|movi) /.test(row)||/ (?:mov|add) x[01238], (?:xzr|x19|sp)/.test(row)||/ cbz x0,/.test(row))continue;
if(/adrp /.test(row)&&!row.includes('//'))continue;
if(/ldr x\d+, \[x\d+, #0x/.test(row)&&!row.includes('//')&&!row.includes('[x19,'))continue;
console.log(row);}
