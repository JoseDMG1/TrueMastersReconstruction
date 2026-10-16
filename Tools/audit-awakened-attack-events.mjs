import fs from 'node:fs';
import crypto from 'node:crypto';

const rosterAuditPath = 'Evidence/roster-moveset-audit.json';
const rosterAudit = JSON.parse(fs.readFileSync(rosterAuditPath, 'utf8').replace(/^\uFEFF/, ''));
const sha256 = file => crypto.createHash('sha256').update(fs.readFileSync(file)).digest('hex');
const fighters = rosterAudit.fighters.map(fighter => ({
  fighter: fighter.name,
  controller: fighter.controller,
  controllerSha256: fighter.controller_sha256,
  callbacks: fighter.states.flatMap(state => (state.clips || []).flatMap(clip => (clip.events || [])
    .filter(event => event.method === 'CreateAwakeAtk')
    .map(event => ({ state: state.name, clip: clip.name, clipFile: clip.file, clipSha256: clip.sha256, time: event.time, receiver: event.object }))))
}));
const withAwakenedAttack = fighters.filter(fighter => fighter.callbacks.length).map(fighter => fighter.fighter);
if (JSON.stringify(withAwakenedAttack) !== JSON.stringify(['Raven', 'Maya']))
  throw new Error(`Unexpected authored CreateAwakeAtk roster: ${withAwakenedAttack.join(', ')}`);
if (fighters.find(fighter => fighter.fighter === 'Raven').callbacks.length !== 3 ||
    fighters.find(fighter => fighter.fighter === 'Maya').callbacks.length !== 2)
  throw new Error('Raven/Maya authored awakened clip callback count changed');
const report = {
  generatedUtc: new Date().toISOString(),
  evidenceType: 'Unity YAML and extracted animation-event static audit',
  rosterAudit: { file: rosterAuditPath, sha256: sha256(rosterAuditPath) },
  method: 'CreateAwakeAtk',
  fightersWithAuthoredCallbacks: withAwakenedAttack,
  fightersWithoutAuthoredCallbacks: fighters.filter(fighter => !fighter.callbacks.length).map(fighter => fighter.fighter),
  fighters
};
fs.writeFileSync('Evidence/awakened-attack-event-audit.json', JSON.stringify(report, null, 2));
console.log(JSON.stringify({ fightersWithAuthoredCallbacks: withAwakenedAttack, events: fighters.filter(f => f.callbacks.length).map(f => ({fighter:f.fighter,count:f.callbacks.length})) }));
