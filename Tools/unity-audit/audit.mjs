import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import { fileURLToPath, pathToFileURL } from 'node:url';
import { parseDocument } from 'yaml';

const project = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../..');
const digest = text => crypto.createHash('sha256').update(text).digest('hex');
const slash = text => text.split(path.sep).join('/');
const refId = ref => ref?.fileID ?? null;

// Unity's document header carries class and object IDs outside its YAML body.
// Parse each body using a real YAML parser; failsafe schema preserves exact IDs
// including leading zero GUIDs and integers outside JS safe range.
export function parseUnity(text) {
  const headers = [...text.matchAll(/^--- !u!(\d+) &(-?\d+)(?: stripped)?\r?$/gm)];
  const documents = [], errors = [];
  let previousOffset = 0, previousLine = 1;
  if (!headers.length) errors.push({ message: 'No Unity YAML document headers found', line: 1 });
  for (let i = 0; i < headers.length; i++) {
    const match = headers[i];
    const offset = match.index + match[0].length;
    const line = previousLine + (text.slice(previousOffset, offset).match(/\n/g)?.length ?? 0);
    previousOffset = offset;
    previousLine = line;
    const body = text.slice(offset, headers[i + 1]?.index ?? text.length);
    const doc = parseDocument(body, { schema: 'failsafe', uniqueKeys: true, prettyErrors: false });
    if (doc.errors.length) {
      errors.push(...doc.errors.map(error => ({ document_id: match[2], line, message: error.message })));
      continue;
    }
    try {
      const data = doc.toJS({ maxAliasCount: 100 });
      const type = Object.keys(data ?? {})[0];
      if (!type || !data[type] || typeof data[type] !== 'object') throw new Error('Document does not contain a Unity object map');
      documents.push({ document_id: match[2], class_id: match[1], type, line, data: data[type] });
    } catch (error) { errors.push({ document_id: match[2], line, message: error.message }); }
  }
  return { documents, errors, document_headers: headers.length };
}

async function allFiles(root) {
  const files = [];
  async function walk(dir) {
    for (const entry of await fs.readdir(dir, { withFileTypes: true })) {
      const target = path.join(dir, entry.name);
      if (entry.isSymbolicLink()) continue;
      if (entry.isDirectory()) await walk(target);
      else if (entry.isFile()) files.push(target);
    }
  }
  await walk(root);
  return files.sort();
}

function extractFields(data) {
  const labels = [], events = [];
  function walk(value, keyPath) {
    if (!value || typeof value !== 'object') return;
    if (typeof value.m_MethodName === 'string' && value.m_MethodName) events.push({ property: keyPath, method: value.m_MethodName,
      target: value.m_Target ?? null, target_assembly_type: value.m_TargetAssemblyTypeName ?? null,
      arguments: value.m_Arguments ?? null, call_state: value.m_CallState ?? null, mode: value.m_Mode ?? null });
    for (const [key, item] of Object.entries(value)) {
      const next = keyPath ? `${keyPath}.${key}` : key;
      if (['m_Text', 'm_text', 'text'].includes(key) && typeof item === 'string') labels.push({ property: next, text: item });
      if (item && typeof item === 'object') walk(item, next);
    }
  }
  walk(data, '');
  return { labels, events };
}

export function analyzeFile(parsed, file, guid, mappings) {
  const byId = new Map(parsed.documents.map(doc => [doc.document_id, doc]));
  const gameObjects = parsed.documents.filter(doc => doc.type === 'GameObject');
  const transforms = new Map(parsed.documents.filter(doc => ['Transform', 'RectTransform'].includes(doc.type)).map(doc => [refId(doc.data.m_GameObject), doc]));
  function objectPath(id, seen = new Set()) {
    if (!id || id === '0') return null;
    if (seen.has(id)) return `[cycle:${id}]`;
    seen.add(id);
    const name = byId.get(id)?.data.m_Name ?? `[unresolved:${id}]`;
    const parentTransform = byId.get(refId(transforms.get(id)?.data.m_Father));
    const parent = refId(parentTransform?.data.m_GameObject);
    return parent && parent !== '0' ? `${objectPath(parent, seen)}/${name}` : name;
  }
  const objects = gameObjects.map(doc => {
    const transform = transforms.get(doc.document_id);
    const parent = byId.get(refId(transform?.data.m_Father));
    return { document_id: doc.document_id, name: doc.data.m_Name ?? null, active: doc.data.m_IsActive ?? null,
      hierarchy_path: objectPath(doc.document_id), transform_id: transform?.document_id ?? null,
      parent_transform_ref: transform?.data.m_Father ?? null, parent_game_object_id: refId(parent?.data.m_GameObject),
      child_transform_refs: transform?.data.m_Children ?? [], component_refs: doc.data.m_Component ?? [] };
  });
  const components = parsed.documents.filter(doc => doc.type === 'MonoBehaviour').map(doc => {
    const script = doc.data.m_Script ?? null;
    const candidates = script?.guid ? mappings.get(script.guid) ?? [] : [];
    const resolution = !script || refId(script) === '0' ? 'missing_reference' : !script.guid ? 'unknown_guid' : !candidates.length ? 'unmapped_guid' : candidates.length > 1 ? 'ambiguous_guid' : 'mapped';
    const fields = extractFields(doc.data);
    for (const event of fields.events) {
      const target = event.target?.guid ? null : byId.get(refId(event.target));
      event.target_resolution = event.target?.guid ? 'external_reference_not_expanded' : target ? 'local_document' : refId(event.target)==='0' ? 'null_target' : 'unresolved_local_document';
      event.target_document_type = target?.type ?? null;
      event.target_hierarchy_path = target ? objectPath(target.type==='GameObject'?target.document_id:refId(target.data.m_GameObject)) : null;
    }
    return { document_id: doc.document_id, game_object_id: refId(doc.data.m_GameObject), hierarchy_path: objectPath(refId(doc.data.m_GameObject)),
      script_ref: script, script_resolution: resolution, script_assets: candidates,
      enabled: doc.data.m_Enabled ?? null, serialized_fields: Object.keys(doc.data).filter(key => !['m_ObjectHideFlags','m_CorrespondingSourceObject','m_PrefabInstance','m_PrefabAsset','m_GameObject','m_Enabled','m_EditorHideFlags','m_Script','m_Name','m_EditorClassIdentifier'].includes(key)),
      ...fields,
      control_fields: Object.fromEntries(Object.entries(doc.data).filter(([key])=> /^(m_Interactable|m_IsOn|m_Value|m_MinValue|m_MaxValue|m_WholeNumbers|m_Direction|m_Options|m_CharacterLimit|m_ContentType|m_LineType|m_Transition|m_Navigation|m_ReadOnly|m_Text|m_text|text)$/.test(key) || /volume|music|sound|quality|language|resolution|sensitivity|fullscreen|difficulty/i.test(key))),
      control_hint: /button|toggle|slider|dropdown|inputfield|setting/i.test(`${candidates.join(' ')} ${objectPath(refId(doc.data.m_GameObject))}`),
    };
  });
  return { file, guid, document_headers: parsed.document_headers, parsed_documents: parsed.documents.length, parse_errors: parsed.errors,
    game_objects: objects, mono_behaviours: components,
    unresolved_parent_refs: objects.filter(object => object.parent_transform_ref?.fileID && object.parent_transform_ref.fileID !== '0' && !byId.has(object.parent_transform_ref.fileID)).map(object => ({ document_id: object.document_id, parent_ref: object.parent_transform_ref })),
    prefab_instances: parsed.documents.filter(doc => doc.type === 'PrefabInstance').map(doc => ({ document_id: doc.document_id, source_prefab: doc.data.m_SourcePrefab ?? null })),
    ui_label_count: components.reduce((sum, component) => sum + component.labels.length, 0),
    persistent_call_count: components.reduce((sum, component) => sum + component.events.length, 0) };
}

export async function audit(root, outputProject = project, { menusOnly = false } = {}) {
  const started = performance.now(), cpu = process.cpuUsage();
  const files = await allFiles(root), mappings = new Map(), metaErrors = [], metaHashes = [], manifest = [];
  const fileGuid = new Map();
  for (const file of files.filter(file => file.endsWith('.meta'))) {
    const bytes = await fs.readFile(file);
    const relative = slash(path.relative(root, file));
    const doc = parseDocument(bytes.toString('utf8'), { schema: 'failsafe', prettyErrors: false });
    if (doc.errors.length) { metaErrors.push({ file: relative, errors: doc.errors.map(error => error.message) }); continue; }
    const value = doc.get('guid');
    if (typeof value !== 'string' || !/^[a-fA-F0-9]{32}$/.test(value)) { metaErrors.push({ file: relative, errors: ['Missing or malformed GUID'] }); continue; }
    const asset = relative.slice(0, -5);
    mappings.set(value, [...(mappings.get(value) ?? []), asset]);
    fileGuid.set(asset, value);
    metaHashes.push({ file: relative, sha256: digest(bytes) });
  }
  const prefabFiles = files.filter(file => file.endsWith('.prefab'));
  const targets = files.filter(file => file.endsWith('.unity') || (file.endsWith('.prefab') && (!menusOnly || /[\\/](ui|chat)[\\/]|friend|player ui|button|toggle|slider|dropdown|inputfield|menu/i.test(file))));
  const audits = [];
  for (const file of targets) {
    const bytes = await fs.readFile(file), relative = slash(path.relative(root, file));
    const parsed = parseUnity(bytes.toString('utf8'));
    manifest.push({ file: relative, sha256: digest(bytes), bytes: bytes.length });
    audits.push(analyzeFile(parsed, relative, fileGuid.get(relative) ?? null, mappings));
  }
  const scenes = audits.filter(item => item.file.endsWith('.unity'));
  const relevantPrefabs = audits.filter(item => item.file.endsWith('.prefab') && (item.ui_label_count || item.persistent_call_count || item.mono_behaviours.some(component => component.control_hint) || /[\\/]ui[\\/]/i.test(item.file)));
  const counts = { scenes: scenes.length, prefabs_discovered: prefabFiles.length, prefabs_examined: audits.length - scenes.length, relevant_prefabs: relevantPrefabs.length,
    yaml_document_headers: audits.reduce((sum,item) => sum + item.document_headers,0), parsed_documents: audits.reduce((sum,item) => sum+item.parsed_documents,0),
    game_objects: audits.reduce((sum,item) => sum+item.game_objects.length,0), mono_behaviours: audits.reduce((sum,item) => sum+item.mono_behaviours.length,0),
    ui_labels: audits.reduce((sum,item) => sum+item.ui_label_count,0), persistent_calls: audits.reduce((sum,item) => sum+item.persistent_call_count,0),
    missing_script_mappings: audits.reduce((sum,item) => sum+item.mono_behaviours.filter(component => component.script_resolution !== 'mapped').length,0),
    parse_errors: audits.reduce((sum,item) => sum+item.parse_errors.length,0), meta_files: metaHashes.length, meta_errors: metaErrors.length };
  const limitations = ['Static exported serialization only; not runtime behavior or completeness of the original game.',
    'Missing labels, persistent calls or custom fields may reflect incomplete IL2CPP export. Absence here does not establish absence in the game.',
    'Mapped script GUIDs establish an exported asset association, not functional recovered implementation.',
    'Prefab instances are reported but overrides/inheritance are not expanded.',
    menusOnly ? 'Menu profile: scenes plus prefabs selected by explicit ui/chat/friend/player UI/button/toggle/slider/dropdown/inputfield/menu path identifiers. Other prefabs not parsed; no completeness claim for UI hidden in those prefabs.' : 'All prefabs were parsed; detailed graphs retained only for scenes and UI/control candidate prefabs.',
    'Names/control hints are static identifiers, not proof of interactability.'];
  const usage = process.cpuUsage(cpu);
  const report = { version:1, root, profile:menusOnly?'menus-only':'all-scenes-and-prefabs', method:'Unity document framing + yaml 2.9.0 failsafe scalar parser', coverage:counts, limitations,
    measured: { elapsed_ms:Math.round(performance.now()-started), cpu_ms:Math.round((usage.user+usage.system)/1000), peak_process_memory_bytes: process.resourceUsage().maxRSS*1024, scope:'single Node process; no comparative benchmark' },
    scene_files:scenes.map(item=>item.file), input_manifest:manifest, meta_manifest:metaHashes, meta_errors:metaErrors,
    duplicate_guids:[...mappings].filter(([,assets])=>assets.length>1).map(([guid,assets])=>({guid,assets})),
    missing_script_references:audits.flatMap(item=>item.mono_behaviours.filter(component=>component.script_resolution!=='mapped').map(component=>({file:item.file,...component}))),
    files:[...scenes,...relevantPrefabs], other_prefab_errors:audits.filter(item=>item.file.endsWith('.prefab')&&!relevantPrefabs.includes(item)&&item.parse_errors.length).map(item=>({file:item.file,errors:item.parse_errors})) };
  await fs.mkdir(path.join(outputProject,'Evidence'),{recursive:true});
  await fs.mkdir(path.join(outputProject,'Knowledge'),{recursive:true});
  await fs.writeFile(path.join(outputProject,'Evidence','unity-menu-audit.json'),JSON.stringify(report,null,2)+'\n');
  const findings=[];
  let markdown='# Auditoría estática de escenas y menús Unity\n\nFuente: `Evidence/unity-menu-audit.json`. Esta inspección lee YAML exportado, sin abrir Unity ni ejecutar scripts.\n\n'+
    `Cobertura: ${counts.scenes} escenas, ${counts.prefabs_examined} prefabs examinados, ${counts.relevant_prefabs} prefabs candidatos de UI con detalle retenido; ${counts.parsed_documents}/${counts.yaml_document_headers} documentos parseados. ${counts.parse_errors} errores YAML y ${counts.meta_errors} errores meta. ${counts.missing_script_mappings} referencias de script sin asociación única.\n\n`+
    `Se encontraron ${counts.ui_labels} textos UI serializados y ${counts.persistent_calls} llamadas persistentes. La falta de estos campos no demuestra que el juego carezca de ellos.\n\n`;
  for (const item of [...scenes,...relevantPrefabs]) {
    const methods=[...new Set(item.mono_behaviours.flatMap(component=>component.events.map(event=>event.method)))];
    const labels=[...new Set(item.mono_behaviours.flatMap(component=>component.labels.map(label=>label.text)).filter(Boolean))];
    const interesting=item.game_objects.filter(object=>/menu|button|toggle|slider|option|setting|canvas|panel|room|chat|character|select|login|sound|music|quality|language|volume|surviv|tournament|play|credit|help|tutorial|connect|friend/i.test(object.name??''));
    markdown+=`## ${item.file}\n\nGUID: \`${item.guid??'unknown'}\`. ${item.game_objects.length} GameObjects, ${item.mono_behaviours.length} MonoBehaviours.\n\n`;
    markdown+=`Identificadores estáticos relevantes (no etiquetas de UI verificadas): ${interesting.map(object=>`\`${object.hierarchy_path}\` [ID ${object.document_id}]`).join('; ')||'ninguno'}.\n\n`;
    markdown+=`Textos serializados: ${labels.map(label=>JSON.stringify(label)).join('; ')||'ninguno recuperado'}. Métodos UnityEvent: ${methods.join(', ')||'ninguno recuperado'}.\n\n`;
    findings.push({id:`unity-static:${item.file}`,statement:`El archivo exportado ${item.file} contiene ${item.game_objects.length} GameObjects y ${item.mono_behaviours.length} MonoBehaviours. Identificadores relevantes: ${interesting.map(object=>object.name).join(', ')||'ninguno'}. Textos UI serializados: ${labels.length}; llamadas persistentes: ${item.persistent_call_count}.`,status:'observed',source_file:path.join(root,item.file),source_sha256:manifest.find(entry=>entry.file===item.file)?.sha256,source_guid:item.guid,tags:['unity','menu','static','scenes'],limitations});
    for(const component of item.mono_behaviours){
      const provenance={status:'observed',source_file:path.join(root,item.file),source_sha256:manifest.find(entry=>entry.file===item.file)?.sha256,source_guid:item.guid,source_document_id:component.document_id,game_object_id:component.game_object_id,hierarchy_path:component.hierarchy_path,limitations:['Static exported serialization; not proof of runtime visibility or behavior.']};
      for(const label of component.labels) if(label.text.trim()) findings.push({...provenance,id:`unity-label:${item.file}:${component.document_id}:${label.property}`,statement:`Texto UI serializado en ${item.file}, ${component.hierarchy_path}, ${label.property}: ${JSON.stringify(label.text)}.`,property:label.property,serialized_text:label.text,tags:['unity','menu','ui','text','static']});
      for(const event of component.events) findings.push({...provenance,id:`unity-event:${item.file}:${component.document_id}:${event.property}`,statement:`UnityEvent serializado en ${item.file}, ${component.hierarchy_path}, ${event.property}: método ${event.method}, destino ${event.target_hierarchy_path??JSON.stringify(event.target)}, modo ${event.mode}, estado ${event.call_state}.`,event,tags:['unity','menu','button','event','static']});
      if(Object.keys(component.control_fields).some(key=>!['m_Text','m_text','text'].includes(key))) findings.push({...provenance,id:`unity-control:${item.file}:${component.document_id}`,statement:`Configuración serializada de control en ${item.file}, ${component.hierarchy_path}: ${JSON.stringify(component.control_fields)}.`,control_fields:component.control_fields,tags:['unity','menu','settings','controls','static']});
    }
  }
  markdown+='## Límites\n\n'+limitations.map(item=>`- ${item}`).join('\n')+'\n';
  await fs.writeFile(path.join(outputProject,'Knowledge','unity-menu-analysis.md'),markdown);
  await fs.writeFile(path.join(outputProject,'Knowledge','unity-menu-findings.json'),JSON.stringify({entries:findings},null,2)+'\n');
  return {coverage:counts,measured:report.measured};
}

if(process.argv[1]&&import.meta.url===pathToFileURL(path.resolve(process.argv[1])).href){
  const args=process.argv.slice(2);
  audit(path.resolve(args.find(arg=>!arg.startsWith('--'))??path.join(project,'Evidence','RecoveredUnityProject','ExportedProject')),project,{menusOnly:args.includes('--menus-only')}).then(result=>console.log(JSON.stringify(result,null,2))).catch(error=>{console.error(error);process.exitCode=1;});
}
