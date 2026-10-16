import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import { build, query, context, tokenize } from './rag.mjs';

async function fixture(t) {
  const root = await fs.mkdtemp(path.join(os.tmpdir(), 'true-masters-rag-'));
  t.after(() => fs.rm(root, { recursive: true, force: true }));
  await fs.writeFile(path.join(root, 'findings.json'), JSON.stringify({ entries: [
    { id: 'fire-test', statement: 'El personaje utiliza fuego en combate.', status: 'observed', source_file: 'C:\\videos\\game.mp4', start_seconds: 42, end_seconds: 46, tags: ['attack'], evidence_id: 'REA:fixture', limitation: 'One sampled scene' },
    { id: 'network-test', statement: 'El protocolo del servidor multijugador es desconocido.', status: 'unknown', source_file: 'C:\\apk\\game.apk', tags: ['network', 'server'] },
  ] }));
  return root;
}

test('bilingual retrieval preserves status, exact source, timing and extra evidence metadata', async t => {
  const root = await fixture(t);
  await build(root);
  const result = await query(root, 'fire combat character attack');
  assert.equal(result.results[0].finding_id, 'fire-test');
  assert.equal(result.results[0].status, 'observed');
  assert.equal(result.results[0].source.source_file, 'C:\\videos\\game.mp4');
  assert.equal(result.results[0].source.start_seconds, 42);
  assert.equal(result.results[0].source.evidence_id, 'REA:fixture');
  assert.match(result.results[0].knowledge_sha256, /^[a-f0-9]{64}$/);
  const unknown = await query(root, 'multiplayer server network');
  assert.equal(unknown.results[0].status, 'unknown');
  assert.match(context(unknown), /\[E1\]/);
  assert.match(context(unknown), /Do not invent/);
  assert.match(context(unknown), /"status":"unknown"/);
  assert.equal((await query(root, 'zebracorn')).matches, 0);
});

test('generated/schema outputs are excluded; markdown keeps source lines; changed inputs require rebuild', async t => {
  const root = await fixture(t);
  await fs.mkdir(path.join(root, 'generated'));
  await fs.mkdir(path.join(root, 'schema'));
  await fs.writeFile(path.join(root, 'generated', 'context.md'), 'loopword');
  await fs.writeFile(path.join(root, 'schema', 'README.md'), 'schemaword');
  await fs.writeFile(path.join(root, 'notes.md'), '# Menus\nOpciones de audio.\n');
  const summary = await build(root);
  assert.equal(summary.files, 2);
  assert.equal((await query(root, 'loopword schemaword')).matches, 0);
  const markdown = (await query(root, 'audio')).results[0];
  assert.equal(markdown.status, 'unknown');
  assert.equal(markdown.source.start_line, 1);
  assert.equal(markdown.knowledge_file, 'notes.md');
  assert.deepEqual(await build(root), summary);
  await fs.appendFile(path.join(root, 'notes.md'), 'New evidence\n');
  await assert.rejects(query(root, 'audio'), /Knowledge changed/);
});

test('malformed findings, invalid chronology and duplicate identities fail visibly', async t => {
  const root = await fixture(t);
  const target = path.join(root, 'bad.json');
  await fs.writeFile(target, JSON.stringify([{id:'bad', statement:'text', status:'confirmed', source_file:'video'}]));
  await assert.rejects(build(root), /valid status/);
  const entry = { id:'bad', statement:'text', status:'observed', source_file:'video', start_seconds:10, end_seconds:1 };
  await fs.writeFile(target, JSON.stringify([entry]));
  await assert.rejects(build(root), /reversed time/);
  delete entry.end_seconds;
  await fs.writeFile(target, JSON.stringify([entry, entry]));
  await assert.rejects(build(root), /Duplicate finding identity/);
});

test('source API identifiers retain exact tokens and expose camelCase/underscore words',()=>{
  const tokens=tokenize('Application.set_targetFrameRate GameManager.CapFrameRate');
  assert.ok(tokens.includes('set_targetframerate'));
  assert.ok(tokens.includes('target'));
  assert.ok(tokens.includes('frame'));
  assert.ok(tokens.includes('rate'));
  assert.ok(tokens.includes('gamemanager'));
  assert.ok(tokens.includes('capframerate'));
});
