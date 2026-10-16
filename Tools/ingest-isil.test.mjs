import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import crypto from 'node:crypto';
import {parseIsil,methodIdentity,ingest} from './ingest-isil.mjs';

test('section ownership and canonical identity preserve exact source while handling nested/default parameters',()=>{
  const parsed=parseIsil('Type: Score+<TimeUp>d__17\n\nMethod: System.Boolean MoveNext()\n\nDisassembly:\n\t0xBAD WRONG_RAW\n\nISIL:\n\t001 Move W0, 1\n\t002 Return\n');
  assert.equal(parsed.methods[0].sections.ISIL.lines[0].line,9);
  assert.equal(parsed.methods[0].sections.Disassembly.lines[0].text,'\t0xBAD WRONG_RAW');
  assert.equal(methodIdentity('AI+Inner','System.Void Test(System.Collections.Generic.Dictionary<System.String, System.Int32> values, System.String key = "x,y", System.Boolean flag = False)'),methodIdentity('AI/Inner','System.Void Test(System.Collections.Generic.Dictionary<System.String, System.Int32> values, System.String key, System.Boolean flag)'));
  assert.equal(parsed.errors.length,0);
});

test('ingestion excludes invalid raw disassembly, binds mapped VA distinctly from raw offset and retains hash/line citations',async t=>{
  const output=await fs.mkdtemp(path.join(os.tmpdir(),'true-masters-isil-'));
  t.after(()=>fs.rm(output,{recursive:true,force:true}));
  const root=path.join(output,'input');await fs.mkdir(root);
  const source='Type: GameManager\n\nMethod: System.Void Start()\n\nDisassembly:\n\t0xBAD WRONG_RAW\n\nISIL:\n\t001 Call Time.set_timeScale, V0\n\t002 Return\n';
  await fs.writeFile(path.join(root,'GameManager.txt'),source);
  await fs.writeFile(path.join(root,'ThirdParty.txt'),'Type: ThirdParty\nMethod: System.Void Run()\nISIL:\n\t001 Return\n');
  const methodMap=path.join(output,'map.json');
  await fs.writeFile(methodMap,JSON.stringify({entries:[{declaring_type:'GameManager',method_signature:'System.Void Start()',native_address:'0x12946AC',raw_file_offset:'0x12906AC',emitted_RVA:'0x12906AC'}]}));
  await fs.mkdir(path.join(output,'Evidence'));
  await fs.writeFile(path.join(output,'Evidence','native-disassembly-GameManager.txt'),'Method: System.Void Start()\nNative VA: 0x12946AC; raw offset: 0x12906AC; length: 0x4\n0x12946AC stp x30, x19, [sp, #-0x10]!\n');
  const report=await ingest({root,output,methodMap});
  assert.equal(report.selected_files,1);assert.equal(report.metadata_bound_methods,1);
  assert.equal(report.corrected_native_methods,1);
  const finding=JSON.parse(await fs.readFile(path.join(output,'Knowledge','implementation-findings.json'),'utf8')).entries[0];
  assert.equal(finding.native_method_address,'0x12946AC');
  assert.equal(finding.raw_file_offset,'0x12906AC');
  assert.equal(finding.source_start_line,9);assert.equal(finding.status,'observed');
  assert.equal(finding.source_sha256,crypto.createHash('sha256').update(source).digest('hex'));
  assert.equal(finding.excluded_invalid_disassembly.status,'invalid_tool_artifact_do_not_ground');
  assert.doesNotMatch(finding.statement,/WRONG_RAW/);
  assert.match(finding.statement,/Time.set_timeScale/);
  const native=JSON.parse(await fs.readFile(path.join(output,'Knowledge','implementation-findings.json'),'utf8')).entries[1];
  assert.match(native.arm64_disassembly,/stp x30/);assert.equal(native.source_start_line,3);
  assert.equal(native.native_method_address,'0x12946AC');
});
