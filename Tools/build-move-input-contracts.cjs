// Development-probe input expectations from the retained format-aware controller audit.
const fs = require('fs');
const crypto = require('crypto');
const path = require('path');
const root = path.resolve(__dirname, '..');
const auditPath = path.join(root, 'Evidence/roster-moveset-audit.json');
const auditBytes = fs.readFileSync(auditPath);
const audit = JSON.parse(auditBytes);
const hash = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const inputs = [[0,0],[0,1],[0,-1],[1,0],[-1,0]];
const routes = [];
for (const fighter of audit.fighters) {
  if (hash(fs.readFileSync(path.join(root, fighter.controller))) !== fighter.controller_sha256)
    throw new Error('Controller changed; regenerate roster audit: '+fighter.controller);
  const machine = fighter.stateMachines.find(m => m.name === 'Attacks');
  if (!machine) throw new Error('Missing Attacks machine: '+fighter.name);
  for (let direction=0; direction<inputs.length; direction++) for (let variant=0; variant<2; variant++) {
    const [Horizontal, Vertical] = inputs[direction];
    const values = {Horizontal, Vertical, BasicAttack:variant, UpAttack:variant,
      DownAttack:variant, RightAttack:variant, LeftAttack:variant, Attack:1};
    const matches = condition => {
      const value = values[condition.m_ConditionEvent];
      if (value === undefined) throw new Error('Unknown entry parameter: '+condition.m_ConditionEvent);
      const threshold = Number(condition.m_EventTreshold);
      switch(Number(condition.m_ConditionMode)) {
        case 1: return !!value;
        case 2: return !value;
        case 3: return value > threshold;
        case 4: return value < threshold;
        case 6: return value === threshold;
        case 7: return value !== threshold;
        default: throw new Error('Unknown AnimatorConditionMode: '+condition.m_ConditionMode);
      }
    };
    const entry = machine.entryTransitions.find(t => t.conditions.every(matches));
    const destination = entry ? entry.destination : machine.defaultState;
    const state = fighter.states.find(s => s.id === destination?.fileID);
    if (!state) throw new Error('Unresolved entry state: '+fighter.name+'/'+direction+'/'+variant);
    routes.push({fighter:fighter.name, direction, variant, state:state.name,
      controller:fighter.controller, controllerSha256:fighter.controller_sha256,
      transitionId:entry?.id ?? null, defaultEntry:!entry});
  }
}
if(routes.length !== 120) throw new Error('Expected twelve fighters and ten input cases each');
const output = path.join(root, 'Tools/unity-validation/MoveInputContracts.json');
fs.writeFileSync(output, JSON.stringify({scope:'Static expected state, not runtime acceptance',
  source:'Evidence/roster-moveset-audit.json', sourceSha256:hash(auditBytes), routes},null,2)+'\n');
console.log(JSON.stringify({routes:routes.length,output}));
