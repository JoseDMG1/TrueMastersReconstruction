import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import { fileURLToPath, pathToFileURL } from 'node:url';

const project=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'..');
const hash=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
const scope=/^(GameManager|GameController|Score|ScoreController|ScoreController_LM|SpecialAtk|StarMenuManager|SystemManager|PlayerController|PlayerController_LM|ButtonsController|ButtonAttack|InputAnimationController|AttackController|AttacksManager|AttacksController_LM|DamagePlayerController|DamageController|BlockingController|CPUController|AI|SelectionController|SurvivalController|TournamentController|OnlineManager|OnlineController|MenuController|LevelChanger|CharacterManager|CharacterController|StageController|MovePlayer|Movement|MoveController|Jump|HitController|StatsController|ResultController|ResultsController|Settings|ChargeSpecial)(?:_NestedType__.*)?$/;
const caveats=[
  'Static lifted ISIL, not original C# and not proof of runtime behavior.',
  'Cpp2IL printed Disassembly is INVALID for this ELF: method virtual address was read as file offset (PT_LOAD delta 0x4000). That section is excluded from grounding; original dump remains available as an invalid tool artifact.',
  'NotImplemented operations, indirect calls, unresolved addresses and branch/lifting semantics may prevent reliable behavioral reconstruction.',
  'Method boundaries follow dump Method headers; native address is accepted only from authoritative metadata mapping, never from invalid Disassembly.',
  'Corrected ARM64 disassembly, when present, is decoded from mapped ELF bytes with Capstone; reported method lengths are tool estimates and may not prove exact end boundaries.',
];

// Dump uses '+' for nested types; Cecil metadata uses '/'. Parameter defaults
// are presentation only. Parse commas outside generic/array/quoted values;
// retain the original signature and reject collisions at lookup time.
export function methodIdentity(type,signature){
  const open=signature.indexOf('('),close=signature.lastIndexOf(')');
  if(open<0||close<open)throw new Error(`Malformed method signature ${signature}`);
  const params=[],body=signature.slice(open+1,close);
  let start=0,depth=0,quote=null,escape=false,defaultAt=-1;
  for(let i=0;i<=body.length;i++){
    const ch=body[i];
    if(quote){if(escape)escape=false;else if(ch==='\\')escape=true;else if(ch===quote)quote=null;continue;}
    if(ch==='"'||ch==="'"){quote=ch;continue;}
    if('(<[{'.includes(ch??'\0'))depth++;
    if(')>]}'.includes(ch??'\0'))depth--;
    if(ch==='='&&depth===0&&defaultAt<0)defaultAt=i;
    if((ch===','&&depth===0)||i===body.length){params.push(body.slice(start,defaultAt>=0?defaultAt:i).trim().replaceAll('+','/'));start=i+1;defaultAt=-1;}
  }
  return `${type.replaceAll('+','/')}::${signature.slice(0,open).replaceAll('+','/')}(${params.join(', ')})`;
}

export function parseIsil(text){
  const lines=text.replace(/^\uFEFF/,'').split(/\r?\n/), methods=[],errors=[];
  let type=null,method=null,section=null;
  function finish(){ if(method){method.end_line=method.end_line??lines.length;methods.push(method);} }
  for(let i=0;i<lines.length;i++){
    const line=lines[i];
    if(line.startsWith('Type: ')){type=line.slice(6);continue;}
    if(line.startsWith('Method: ')){
      if(method)method.end_line=i;
      finish();method={type,signature:line.slice(8),start_line:i+1,sections:{}};section=null;continue;
    }
    if(['Disassembly:','ISIL:'].includes(line)){
      if(!method){errors.push({line:i+1,message:'Section before Method header'});continue;}
      const key=line.slice(0,-1);
      if(method.sections[key])errors.push({line:i+1,message:`Duplicate ${key} section`});
      section={header_line:i+1,lines:[]};method.sections[key]=section;continue;
    }
    if(method&&section&&line.trim())section.lines.push({line:i+1,text:line});
    else if(line.trim()&&!method&&!line.startsWith('Type: '))errors.push({line:i+1,message:'Unknown preamble'});
  }
  finish();
  for(const item of methods)if(!item.type||!item.sections.ISIL)errors.push({line:item.start_line,message:`Missing type or ISIL for ${item.signature}`});
  return {methods,errors};
}

export function parseCorrectedNative(text,type){
  const methods=[];let current=null;
  for(const [i,line] of text.replace(/^\uFEFF/,'').split(/\r?\n/).entries()){
    if(line.startsWith('Method: ')){current={type,signature:line.slice(8),start_line:i+1,lines:[]};methods.push(current);continue;}
    const meta=line.match(/^Native VA: (0x[\dA-Fa-f]+); raw offset: (0x[\dA-Fa-f]+); length: (0x[\dA-Fa-f]+)$/);
    if(meta&&current){current.native_address=meta[1];current.raw_file_offset=meta[2];current.length=meta[3];continue;}
    if(/^0x[\dA-Fa-f]+\s/.test(line)&&current)current.lines.push({line:i+1,text:line});
  }
  return methods;
}

function summarizeMethod(method){
  const lines=method.sections.ISIL?.lines??[];
  const calls=[...new Set(lines.flatMap(line=>{const match=line.text.match(/^\s*\d+\s+Call(?:NoReturn)?\s+([^,\s]+)/);return match?[match[1]]:[];}))];
  const unsupported=lines.filter(line=>/\bNotImplemented\b/.test(line.text));
  return {calls,unsupported:unsupported.length,instruction_lines:lines.length};
}

export async function ingest({root=path.join(project,'Evidence','Cpp2IL-ISIL','IsilDump','Assembly-CSharp'),methodMap=null,output=project}={}){
  const started=performance.now(),cpu=process.cpuUsage();
  const all=(await fs.readdir(root,{withFileTypes:true})).filter(entry=>entry.isFile()&&entry.name.endsWith('.txt'));
  const selected=all.filter(entry=>scope.test(entry.name.slice(0,-4))).sort((a,b)=>a.name.localeCompare(b.name,'en'));
  const methodMapBytes=methodMap?await fs.readFile(methodMap):null;
  const mapping=methodMapBytes?JSON.parse(methodMapBytes.toString('utf8')):null;
  const methodMapHash=methodMapBytes?hash(methodMapBytes):null;
  // Producer contract is explicitly normalized at the owning boundary; do not
  // guess addresses from disassembly, method order, short names or catalog order.
  const addressEntries=mapping?.entries??[];
  const addresses=new Map();
  for(const entry of addressEntries){
    if(typeof entry.declaring_type!=='string'||typeof entry.method_signature!=='string'||(entry.native_address!==null&&typeof entry.native_address!=='string'))throw new Error('Method address map requires declaring_type, method_signature, native_address string or explicit null');
    const key=methodIdentity(entry.declaring_type,entry.method_signature);
    addresses.set(key,[...(addresses.get(key)??[]),entry]);
  }
  const entries=[],manifest=[],errors=[],summaries=[],nativeManifest=[];
  for(const file of selected){
    const full=path.join(root,file.name),bytes=await fs.readFile(full),sha=hash(bytes),parsed=parseIsil(bytes.toString('utf8'));
    manifest.push({file:full,sha256:sha,methods:parsed.methods.length});
    errors.push(...parsed.errors.map(error=>({file:full,...error})));
    const nativeFile=path.join(output,'Evidence',`native-disassembly-${file.name.slice(0,-4)}.txt`);
    let nativeBytes=null;
    try{nativeBytes=await fs.readFile(nativeFile);}catch(error){if(error.code!=='ENOENT')throw error;}
    const nativeMethods=new Map();
    if(nativeBytes){
      nativeManifest.push({file:nativeFile,sha256:hash(nativeBytes)});
      for(const native of parseCorrectedNative(nativeBytes.toString('utf8'),parsed.methods[0]?.type??file.name.slice(0,-4))){
        const key=methodIdentity(native.type,native.signature);
        nativeMethods.set(key,[...(nativeMethods.get(key)??[]),native]);
      }
    }
    for(const method of parsed.methods){
      const details=summarizeMethod(method), candidates=addresses.get(methodIdentity(method.type,method.signature))??[],record=candidates.length===1?candidates[0]:null,address=record?.native_address?record:null;
      const provenance={source_file:full,source_relative_path:path.relative(output,full).split(path.sep).join('/'),source_sha256:sha,
        declaring_type:method.type,method_signature:method.signature,method_start_line:method.start_line,method_end_line:method.end_line,
        native_method_address:address?.native_address??null,raw_file_offset:address?.raw_file_offset??null,
        address_status:address?'metadata_bound':record?'metadata_explicit_no_native_address':candidates.length>1?'ambiguous_metadata_mapping':'unknown_no_exact_metadata_mapping',
        address_identity_rule:'nested + and / canonicalized; parsed parameter defaults omitted; return type, method name and typed named parameters retained; ambiguous matches unresolved',
        address_provenance:record,method_map_file:methodMap,method_map_sha256:methodMapHash,
        native_binary_sha256:mapping?.sourceBinarySha256??null,
        excluded_invalid_disassembly:method.sections.Disassembly?{header_line:method.sections.Disassembly.header_line,end_line:method.sections.Disassembly.lines.at(-1)?.line??method.sections.Disassembly.header_line,status:'invalid_tool_artifact_do_not_ground'}:null,
        limitations:caveats};
      const nativeCandidates=nativeMethods.get(methodIdentity(method.type,method.signature))??[],native=nativeCandidates.length===1?nativeCandidates[0]:null;
      const validNative=native&&address&&native.native_address&&native.raw_file_offset&&BigInt(native.native_address)===BigInt(address.native_address)&&BigInt(native.raw_file_offset)===BigInt(address.raw_file_offset);
      if(native&&!validNative)errors.push({file:nativeFile,line:native.start_line,message:'Corrected native section VA/raw mapping does not match authoritative metadata; excluded'});
      summaries.push({...provenance,...details,corrected_native_file:validNative?nativeFile:null});
      const lines=method.sections.ISIL?.lines??[];
      for(let start=0;start<lines.length;start+=48){
        const block=lines.slice(start,start+48),raw=block.map(line=>line.text).join('\n');
        entries.push({...provenance,id:`isil:${file.name}:${method.start_line}:${start}`,status:'observed',
          statement:`ISIL estático levantado por Cpp2IL: ${method.type}.${method.signature}; VA ${address?.native_address??'desconocida sin vínculo exacto'}; líneas ${block[0].line}-${block.at(-1).line}. No es C# original ni comportamiento probado. Disassembly impreso inválido excluido.\n${raw}`,
          source_start_line:block[0].line,source_end_line:block.at(-1).line,isil:raw,chunk_ordinal:start/48,
          method_instruction_lines:lines.length,method_not_implemented_count:details.unsupported,
          tags:['unity','implementation','isil','static',method.type,method.signature]});
      }
      if(validNative)for(let start=0;start<native.lines.length;start+=48){
        const block=native.lines.slice(start,start+48),raw=block.map(line=>line.text).join('\n');
        entries.push({...provenance,id:`arm64:${file.name}:${method.start_line}:${start}`,status:'observed',
          statement:`ARM64 estático corregido (Capstone; bytes ELF mapeados): ${method.type}.${method.signature}; VA ${address.native_address}; líneas ${block[0].line}-${block.at(-1).line}. No es C# original ni comportamiento probado. Longitud estimada por herramienta.\n${raw}`,
          source_file:nativeFile,source_relative_path:path.relative(output,nativeFile).split(path.sep).join('/'),source_sha256:hash(nativeBytes),
          source_start_line:block[0].line,source_end_line:block.at(-1).line,arm64_disassembly:raw,
          isil_source_file:full,isil_source_sha256:sha,tags:['unity','implementation','arm64','native','static',method.type,method.signature]});
      }
    }
  }
  await fs.mkdir(path.join(output,'Knowledge'),{recursive:true});
  await fs.mkdir(path.join(output,'Evidence'),{recursive:true});
  await fs.writeFile(path.join(output,'Knowledge','implementation-findings.json'),JSON.stringify({entries},null,2)+'\n');
  const used=process.cpuUsage(cpu);
  const report={version:1,root,selection:scope.source,top_level_txt_discovered:all.length,selected_files:selected.length,selected_methods:summaries.length,findings:entries.length,
    metadata_bound_methods:summaries.filter(item=>item.native_method_address).length,corrected_native_methods:summaries.filter(item=>item.corrected_native_file).length,errors,limitations:caveats,manifest,native_manifest:nativeManifest,methods:summaries,
    measured:{elapsed_ms:Math.round(performance.now()-started),cpu_ms:Math.round((used.user+used.system)/1000),peak_process_memory_bytes:process.resourceUsage().maxRSS*1024}};
  await fs.writeFile(path.join(output,'Evidence','implementation-ingestion.json'),JSON.stringify(report,null,2)+'\n');
  let markdown='# Evidencia estática de implementación IL2CPP\n\n'+caveats.map(item=>`- ${item}`).join('\n')+'\n\n'+
    `Selección explícita: ${selected.length}/${all.length} tipos raíz, ${summaries.length} métodos, ${entries.length} fragmentos ISIL. Tipos de terceros en subdirectorios quedan excluidos. Los tipos excluidos y métodos sin ISIL no se consideran analizados. ${report.metadata_bound_methods} métodos tienen VA enlazada exactamente a metadatos. ${errors.length} errores de formato.\n\n`;
  for(const type of ['GameManager','Score','SpecialAtk','StarMenuManager','GameController','PlayerController_LM','ButtonsController','AttackController','BlockingController','CPUController']){
    markdown+=`## ${type}\n\n`;
    for(const method of summaries.filter(item=>item.declaring_type===type)){
      const source=`${method.source_relative_path}:${method.method_start_line}`;
      markdown+=`- \`${method.method_signature}\` — [dump fuente](../${method.source_relative_path.replaceAll(' ','%20')}), líneas ${method.method_start_line}-${method.method_end_line}; SHA-256 \`${method.source_sha256}\`; VA \`${method.native_method_address??'unknown'}\`. ${method.instruction_lines} líneas ISIL, ${method.unsupported} NotImplemented. Calls explícitos: ${method.calls.join(', ')||'ninguno'}.\n`;
    }
    markdown+='\n';
  }
  markdown+='## Reglas candidatas y límites\n\nEstas observaciones son sobre el texto IR, no reglas de juego verificadas. Los calls simbólicos facilitan hipótesis de implementación; requieren revisar ramas, offsets de campos y muestras en ejecución antes de fijar comportamiento.\n\n'+
    '- GameManager.Start contiene calls Time.set_timeScale, setResolutions, setInitialQualityGraphics y setInitialFrameCap: evidencia de rutas de configuración, sin establecer valores finales de cada rama.\n'+
    '- GameManager.CapFrameRate contiene constantes 30 y 60, llamadas Application.set_targetFrameRate, QualitySettings.set_vSyncCount y PlayerPrefs.SetInt. La asociación completa entre parámetro cap, ramas, vsync y valores persistidos permanece por verificar; no imponer una fórmula a partir del listado lineal.\n'+
    '- SpecialAtk.ChargeSpecial contiene calls Image.set_fillAmount y AudioSource.Play, junto con NotImplemented FCMP, SCVTF, FDIV, MOVI y BLR. Sugiere carga visual/sonora (inferencia), pero la fórmula de carga, el umbral y todas las rutas no están establecidos. El tipo recuperado se llama SpecialAtk, no SpecialAttack.\n'+
    '- Score.UpdateScore contiene Int32.ToString, OnlineManager.IsSpectator y Score.UpdateSpectatorScore. Sugiere rutas para presentación/espectador (inferencia); puntuaciones exactas y condiciones de victoria requieren análisis de offsets, corutinas y ramas adicionales.\n'+
    '- StarMenuManager.SurvivalMode contiene Move W1, 3 antes de LevelChanger.FadeToLevel y Move W8, 1 antes de escritura de campo y SystemManager.RefreshPlayingMode. Son operandos observados en IR; identificar índice de escena 3 y enum de modo requiere corroboración con orden de escenas y campos.\n'+
    '- PlayerController contiene solamente constructor en este dump; no atribuirle movimiento completo. Los métodos específicos de GameController, PlayerController_LM, ButtonsController, BlockingController y AttackController se enumeran arriba para siguientes pases.\n\n'+
    'Los hashes y rangos completos están en Evidence/implementation-ingestion.json. Knowledge/implementation-findings.json contiene fragmentos citables con líneas y metadatos nativos. El Disassembly erróneo no está indexado; permanece únicamente en el dump original para auditar el fallo de la herramienta.\n';
  await fs.writeFile(path.join(output,'Knowledge','implementation-analysis.md'),markdown);
  return {selected_files:report.selected_files,selected_methods:report.selected_methods,findings:entries.length,metadata_bound_methods:report.metadata_bound_methods,corrected_native_methods:report.corrected_native_methods,errors:errors.length,measured:report.measured};
}

if(process.argv[1]&&import.meta.url===pathToFileURL(path.resolve(process.argv[1])).href){
  const mapIndex=process.argv.indexOf('--method-map');
  ingest({methodMap:mapIndex>=0?path.resolve(process.argv[mapIndex+1]):null}).then(result=>console.log(JSON.stringify(result,null,2))).catch(error=>{console.error(error);process.exitCode=1;});
}
