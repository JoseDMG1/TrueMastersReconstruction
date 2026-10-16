// Read exact IL2CPP metadata usage slots. Format: metadata39, ELF64 LE ARM64.
// Layout authority: LibCpp2IL/BinaryStructures/Il2CppMetadataRegistration.cs,
// Il2CppMethodSpec.cs (upstream development; pre-108 fixed-width branch).
const fs = require('node:fs'), path = require('node:path');
const root = path.resolve(__dirname, '..'), evidence = path.join(root, 'Evidence');
const map = JSON.parse(fs.readFileSync(path.join(evidence, 'cpp2il-native-address-map.json')));
const metadata = JSON.parse(fs.readFileSync(path.join(evidence, 'assembly-csharp-metadata.json')));
const b = fs.readFileSync(path.join(evidence, 'acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so'));
const m = fs.readFileSync(path.join(evidence, 'apk-selected/assets/bin/Data/Managed/Metadata/global-metadata.dat'));
const raw = va => { const s = map.segments.find(s => va >= s.virtualAddress && va < s.virtualAddress+s.fileSize); if(!s)throw Error('Unmapped VA '+hex(va)); return va-s.virtualAddress+s.fileOffset; };
const reloc = new Map(), hex = v => '0x'+v.toString(16).toUpperCase();
const sectionOffset=Number(b.readBigUInt64LE(40)), sectionSize=b.readUInt16LE(58), sectionCount=b.readUInt16LE(60);
for(let i=0;i<sectionCount;i++) { const p=sectionOffset+i*sectionSize; if(b.readUInt32LE(p+4)!==4)continue; const start=Number(b.readBigUInt64LE(p+24)), size=Number(b.readBigUInt64LE(p+32)); for(let q=start;q<start+size;q+=24) if(Number(b.readBigUInt64LE(q+8)&0xffffffffn)===1027)reloc.set(Number(b.readBigUInt64LE(q)),Number(b.readBigInt64LE(q+16))); }
const ptr = va => reloc.get(va) ?? Number(b.readBigUInt64LE(raw(va)));
const reg=0x279CAF0;
const instCount=Number(b.readBigUInt64LE(raw(reg+16))), instTable=ptr(reg+24);
const specsCount=Number(b.readBigUInt64LE(raw(reg+64))), specs=ptr(reg+72);
const str = index => {if(index<0||index>=metadata.sections.strings.size)throw Error('String index out of bounds'); const p=metadata.sections.strings.offset+index, end=m.indexOf(0,p); return m.toString('utf8',p,end);};
function typeName(index) { if(index<0||index>=metadata.sections.typeDefinitions.count)throw Error('Type index out of bounds: '+index); const p=metadata.sections.typeDefinitions.offset+index*76; const ns=str(m.readInt32LE(p+4)); return (ns?ns+'.':'')+str(m.readInt32LE(p)); }
function instance(index) {
  if(index===-1)return []; if(index<0||index>=instCount)throw Error('Generic inst out of bounds');
  const p=ptr(instTable+index*8), count=b.readUInt32LE(raw(p)), argv=ptr(p+8);
  if(count>64)throw Error('Unexpected generic parameter count');
  return Array.from({length:count},(_,i)=>{ const t=ptr(argv+i*8), data=ptr(t), bits=b.readUInt32LE(raw(t)+8), kind=(bits>>>16)&255; return {kind,index:data,name:[17,18].includes(kind)?typeName(data):null}; });
}
const entries=process.argv.slice(2).map(input=>{
  const slot=parseInt(input,16), target=ptr(slot), encoded=b.readUInt32LE(raw(target)), kind=encoded>>>29, index=(encoded&0x1fffffff)>>>1;
  if(kind!==6)throw Error('Expected generic method usage kind6 at '+input+', got '+kind);
  if(index>=specsCount)throw Error('MethodSpec out of bounds');
  const p=raw(specs+index*12), defIndex=b.readInt32LE(p), classInst=b.readInt32LE(p+4), methodInst=b.readInt32LE(p+8);
  if(defIndex<0||defIndex>=metadata.sections.methods.count)throw Error('Method definition out of bounds');
  const md=metadata.sections.methods.offset+defIndex*28;
  return {slot:hex(slot),metadata_usage_index:index,method_definition_index:defIndex,declaring_type:typeName(m.readUInt16LE(md+4)),method:str(m.readInt32LE(md)),class_arguments:instance(classInst),method_arguments:instance(methodInst)};
});
console.log(JSON.stringify(entries,null,2));
