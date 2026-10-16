const fs=require('fs'),path=require('path');
const root=path.resolve(__dirname,'..'),map=require(root+'/Evidence/cpp2il-native-address-map.json');
const binary=fs.readFileSync(root+'/Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so');
const targets=process.argv.slice(2).map(v=>parseInt(v,16));
const results=targets.map(target=>({target:'0x'+target.toString(16),direct_calls:[]}));
for(const method of map.entries){
 if(!method.raw_file_offset||!method.length)continue;
 const raw=parseInt(method.raw_file_offset,16),va=parseInt(method.native_address,16),length=parseInt(method.length,16);
 for(let i=0;i+4<=length;i+=4){const instruction=binary.readUInt32LE(raw+i);const opcode=instruction>>>26;if(opcode!==0x25&&opcode!==0x05)continue;
  let imm=instruction&0x03ffffff;if(imm&0x02000000)imm-=0x04000000;
  const target=va+i+imm*4,result=results.find(r=>parseInt(r.target,16)===target);
  if(result)result.direct_calls.push({type:opcode===0x25?'BL':'B',site:'0x'+(va+i).toString(16),owner:method.declaring_type,method:method.method_signature});
 }
}
console.log(JSON.stringify({source_sha256:'d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c',coverage:'Mapped Assembly-CSharp native method bodies only; no virtual/delegate/reflection or engine-method scan',results},null,2));
