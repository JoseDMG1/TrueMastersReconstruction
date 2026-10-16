import test from 'node:test';
import assert from 'node:assert/strict';
import {rewriteScriptRefs,dllFileId} from './restore-ui-package.mjs';

test('exact UI script span replacement preserves every other YAML field and reference',()=>{
  const old='aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa',target='bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb';
  const original=`%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!114 &90071992547409999\nMonoBehaviour:\n  m_GameObject: {fileID: 90071992547409998}\n  m_Script: {fileID: -12, guid: ${old}, type: 3}\n  m_Text: "literal ${old}"\n  m_OnClick:\n    m_PersistentCalls:\n      m_Calls:\n      - m_Target: {fileID: 9876543210123456789}\n        m_MethodName: Start\n`.replaceAll('\n','\r\n');
  const mappings=new Map([[old,new Map([['-12',[{full_name:'UnityEngine.UI.Text',target_guid:target,target_source:'Runtime/Text.cs',serialized_names:['m_Text','m_OnClick']}]]])]]);
  const result=rewriteScriptRefs(original,mappings);
  assert.equal(result.text,original.replace(`{fileID: -12, guid: ${old}, type: 3}`,`{fileID: 11500000, guid: ${target}, type: 3}`));
  assert.equal(result.changes[0].document_id,'90071992547409999');
  assert.deepEqual(result.changes[0].unknown_serialized_fields,[]);
  assert.throws(()=>rewriteScriptRefs(original,new Map([[old,new Map()]])),/Unresolved\/ambiguous/);
});

test('DLL IDs agree with recovered UI references; no hash fallback',()=>{
  assert.equal(dllFileId('UnityEngine.UI','Text'),'708705254');
  assert.equal(dllFileId('UnityEngine.EventSystems','EventSystem'),'-619905303');
  assert.equal(dllFileId('TMPro','TMP_Settings'),'-395462249');
});

test('animation curve and binding script PPtrs migrate while keyframes and unrelated PPtrs retain their exact text',()=>{
  const old='aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa',target='bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb';
  const source=`%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!74 &7400000\nAnimationClip:\n  m_Name: Counter\n  m_FloatCurves:\n  - curve:\n      m_Curve:\n      - time: 0.123456789\n        value: 1\n    attribute: m_Color.a\n    script: {fileID: 708705254, guid: ${old}, type: 3}\n  m_ClipBindingConstant:\n    genericBindings:\n    - path: 1234\n      script: {fileID: 708705254, guid: ${old}, type: 3}\n  objectReference: {fileID: 9876543210123456789}\n`;
  const mappings=new Map([[old,new Map([['708705254',[{full_name:'UnityEngine.UI.Text',target_guid:target,serialized_names:[]}]]])]]);
  const result=rewriteScriptRefs(source,mappings);
  assert.equal(result.changes.length,2);
  assert.equal(result.text,source.replaceAll(`{fileID: 708705254, guid: ${old}, type: 3}`,`{fileID: 11500000, guid: ${target}, type: 3}`));
  assert.throws(()=>rewriteScriptRefs(source.replace('objectReference: {fileID: 9876543210123456789}',`objectReference: {fileID: 708705254, guid: ${old}, type: 3}`),mappings),/Unexpected UI DLL reference/);
});
