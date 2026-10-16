const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const root=path.resolve(__dirname,'..'),map=require(root+'/Evidence/cpp2il-native-address-map.json');
const digest=p=>crypto.createHash('sha256').update(fs.readFileSync(root+'/'+p)).digest('hex');
function walk(p){return fs.readdirSync(p,{withFileTypes:true}).flatMap(e=>e.isDirectory()?walk(path.join(p,e.name)):[path.join(p,e.name)]);}
const reads=[];for(const p of walk(root+'/UnityProject/Assets/Scripts/Assembly-CSharp').filter(p=>p.endsWith('.cs'))){const text=fs.readFileSync(p,'utf8');for(const m of text.matchAll(/\.ReleaseBy\b/g))reads.push({path:path.relative(root,p),line:text.slice(0,m.index).split('\n').length});}
const bindings=[
 {owner:'DamageOnCollision.OnTriggerEnter',field_read_sites:['0x1284058'],consumers:['0x12840EC -> HitAnimation(effect)','0x1284124 -> exception List.Contains(effect)']},
 {owner:'DestroyIfSameObjIsNear.DestroyNearAttack',field_read_sites:['0x1227944','0x1227964'],consumers:['effect equality between incoming and own Description']},
 {owner:'DestroyOnCollision.OnTriggerEnter',field_read_sites:['0x128A254'],consumers:['byTheSameObject and incoming.effect == ShadowTramp'],literal_slots:['0x28168C8']},
 {owner:'SummonAttack.CreateKickAttack',field_read_sites:['0x12B5CB8','0x12B5E08'],consumers:['LeftAttack/RightAttack effect branch'],literal_slots:['0x28157E8','0x28157D8']}
];
const owners=['DamageOnCollision','DestroyIfSameObjIsNear','DestroyOnCollision','SummonAttack'];
const sources=owners.map(name=>{const cs='UnityProject/Assets/Scripts/Assembly-CSharp/'+name+'.cs',native='Evidence/native-disassembly-'+name+'.txt';return {cs,sha256:digest(cs),meta_sha256:digest(cs+'.meta'),native,native_sha256:digest(native)};});
const literals=JSON.parse(cp.execFileSync(process.execPath,[root+'/Tools/resolve-native-usage.cjs','0x28168C8','0x28157E8','0x28157D8'],{encoding:'utf8'}));
fs.writeFileSync(root+'/Evidence/description-field-consumer-audit.json',JSON.stringify({status:'static native-derived correction; new runtime checks pending',native_sha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',metadata_fields:map.fields.filter(f=>f.declaring_type==='Description'&&['effect','ReleaseBy'].includes(f.name)),bindings,literals,sources,remaining_implemented_releaseby_reads:reads,limitations:['No native runtime execution.','Search covers existing Assembly-CSharp C# source only, not unimplemented/recovered native consumers.','No ReleaseBy field renaming, asset rewrite or inference that ReleaseBy lacks original semantics.']},null,2));
console.log(JSON.stringify({corrected_bindings:bindings.length,remaining_implemented_releaseby_reads:reads},null,2));
