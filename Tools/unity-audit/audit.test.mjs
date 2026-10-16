import test from 'node:test';
import assert from 'node:assert/strict';
import { parseUnity, analyzeFile } from './audit.mjs';

test('Unity IDs and GUIDs remain exact; hierarchy, script and persistent UI event retain provenance',()=>{
  const text=`%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &90071992547409999
GameObject:
  m_Name: Settings
  m_IsActive: 1
--- !u!224 &2
RectTransform:
  m_GameObject: {fileID: 90071992547409999}
  m_Father: {fileID: 0}
--- !u!114 &3
MonoBehaviour:
  m_GameObject: {fileID: 90071992547409999}
  m_Script: {fileID: 11500000, guid: 00000000000000000000000000000001, type: 3}
  m_Text: "Audio: on"
  m_OnClick:
    m_PersistentCalls:
      m_Calls:
      - m_Target: {fileID: 3}
        m_MethodName: SetVolume
        m_CallState: 2
`;
  const parsed=parseUnity(text);
  assert.deepEqual(parsed.errors,[]);
  const result=analyzeFile(parsed,'Assets/Settings.unity','scene-guid',new Map([['00000000000000000000000000000001',['Assets/Scripts/Button.cs']]]));
  assert.equal(result.game_objects[0].document_id,'90071992547409999');
  assert.equal(result.game_objects[0].hierarchy_path,'Settings');
  assert.equal(result.mono_behaviours[0].script_ref.guid,'00000000000000000000000000000001');
  assert.equal(result.mono_behaviours[0].script_resolution,'mapped');
  assert.equal(result.mono_behaviours[0].labels[0].text,'Audio: on');
  assert.equal(result.mono_behaviours[0].events[0].method,'SetVolume');
  assert.equal(result.mono_behaviours[0].events[0].target.fileID,'3');
});

test('malformed document does not erase valid partial evidence; absent mapping stays explicit',()=>{
  const parsed=parseUnity(`--- !u!1 &1
GameObject:
  m_Name: Root
--- !u!114 &2
MonoBehaviour:
  m_GameObject: {fileID: 1}
  m_Script: {fileID: 11500000, guid: abc, type: 3}
--- !u!1 &3
GameObject:
  m_Name: [broken
`);
  assert.equal(parsed.documents.length,2);
  assert.equal(parsed.errors.length,1);
  assert.equal(parsed.errors[0].document_id,'3');
  const result=analyzeFile(parsed,'broken.unity',null,new Map());
  assert.equal(result.mono_behaviours[0].script_resolution,'unmapped_guid');
  assert.equal(result.ui_label_count,0);
  assert.equal(result.persistent_call_count,0);
});
