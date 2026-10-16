import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import assert from 'node:assert/strict';
import {fileURLToPath} from 'node:url';

const root=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'..');
const [runtimeArg,buildArg,regressionArg]=process.argv.slice(2);
assert(runtimeArg && buildArg,'Supply explicit main runtime and archived build JSON paths.');
const resolve=p=>path.resolve(root,p);
const read=p=>JSON.parse(fs.readFileSync(resolve(p),'utf8').replace(/^\uFEFF/,''));
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(resolve(p))).digest('hex');
const identity=p=>({path:path.relative(root,resolve(p)).replaceAll('\\','/'),sha256:hash(p)});
const runtime=read(runtimeArg), build=read(buildArg);
const tracePath=runtimeArg.replace(/\.json$/,'.flight.json');
const launchPath=runtimeArg.replace(/\.json$/,'-launch.json');
const trace=read(tracePath),launch=read(launchPath);
assert.equal(runtime.result,'passed'); assert.deepEqual(runtime.errors,[]);
assert.equal(build.result,'Succeeded'); assert.equal(build.errors,0);
assert.equal(launch.Scenario,'MovingTarget'); assert.equal(launch.Fighter,'Sora');
assert.equal(hash(launch.GameplayAssembly),launch.GameplayAssemblySha256);
assert.equal(trace.result,'passed'); assert.deepEqual(trace.errors,[]); assert(trace.restored);
assert(runtime.observations.some(o=>o.step==='fixture-restored'));
assert.equal(runtime.observations.filter(o=>o.step==='directional-move').length,2);
assert(trace.samples.length>=5 && trace.turnFrames>=3);
assert(trace.targetTravel>=1.9 && trace.maximumTargetDrift<=.01);
assert(trace.maximumRotationError<=.15 && trace.samples.every(s=>s.correctTarget));
assert(trace.finalDamage>trace.initialDamage);
assert(trace.damageEvents.some(h=>h.damage>trace.initialDamage && trace.contacts.some(c=>Math.abs(c-h.frame)<=1)));
assert(trace.samples.filter(s=>s.time<=.3 && s.frame<Math.min(...trace.contacts) && Math.abs(s.requestedTurn)>.01).length>=3,'Require corrective turns while the target moves, before impact.');
for(const s of trace.samples) {
    // Independent planar cross-dot check; normal Sora launch is horizontal.
    assert(Math.abs(s.forwardBefore.y)<.001);
    const delta={x:s.commandedTarget.x-s.projectileBefore.x,z:s.commandedTarget.z-s.projectileBefore.z};
    const dot=s.forwardBefore.z*delta.x-s.forwardBefore.x*delta.z;
    assert(Math.abs(s.turnDot-dot)<.001,'Recorded turn direction differs from native planar cross-dot.');
    const requested=(dot>.1?1:dot<-.1?-1:0)*s.speed*s.deltaTime;
    assert(Math.abs(requested-s.requestedTurn)<.001);
    assert(Math.abs(s.speed-1.5*52.75)<.001,'Authored Follow speed was changed.');
}
const sources=['OfflinePlayerProbe.cs','OfflineMovingTargetProbe.cs'];
for(const source of sources) assert.equal(hash('Tools/unity-validation/'+source),hash('UnityProject/Assets/ReconstructionValidation/'+source));
let regression=null;
if(regressionArg) {
    const ordinary=read(regressionArg),ordinaryLaunchPath=regressionArg.replace(/\.json$/,'-launch.json'),ordinaryLaunch=read(ordinaryLaunchPath);
    assert.equal(ordinary.result,'passed'); assert.deepEqual(ordinary.errors,[]);
    assert.equal(ordinaryLaunch.Scenario,'AbilityAim'); assert.equal(ordinaryLaunch.Fighter,'Sora');
    assert.equal(ordinaryLaunch.GameplayAssemblySha256,launch.GameplayAssemblySha256);
    assert.equal(ordinary.observations.filter(o=>o.step==='directional-move').length,10);
    assert(ordinary.observations.some(o=>o.step==='fixture-restored'));
    regression={runtime:identity(regressionArg),launch:identity(ordinaryLaunchPath),normalRoutes:10};
}
const contract={
    generatedUtc:new Date().toISOString(),result:'passed-bounded-moving-target-flight',
    runtime:identity(runtimeArg),trace:identity(tracePath),launch:identity(launchPath),build:identity(buildArg),
    assembly:identity(launch.GameplayAssembly),
    regression,
    sources:[...sources.map(s=>'Tools/unity-validation/'+s),'Tools/Run-OfflineProbe.ps1','Tools/Build-OfflinePlayer.ps1','Tools/audit-moving-target.mjs','UnityProject/Assets/Scripts/Assembly-CSharp/Follow.cs','UnityProject/Assets/Scripts/Assembly-CSharp/GoForward.cs','UnityProject/Assets/Scripts/Assembly-CSharp/DamageOnCollision.cs','UnityProject/Assets/Resources/photonprefabs/AirAtkStrong2.prefab'].map(identity),
    nativeEvidence:['Evidence/native-disassembly-Follow.txt','Evidence/native-disassembly-GoForward.txt','Knowledge/projectile-reconstruction.md'].map(identity),
    measurements:{samples:trace.samples.length,turnFrames:trace.turnFrames,targetTravel:trace.targetTravel,maximumTargetDrift:trace.maximumTargetDrift,maximumRotationError:trace.maximumRotationError,contacts:trace.contacts,damageEvents:trace.damageEvents,initialDamage:trace.initialDamage,finalDamage:trace.finalDamage},
    limitation:trace.limitation,
    evidenceBoundary:'Native listings are retained local Capstone/metadata observations following the REA skill; no connected REA MCP Evidence is claimed. Previous all-roster acceptance remains bound to its earlier assembly.'
};
fs.writeFileSync(resolve('Evidence/moving-target-flight-acceptance.json'),JSON.stringify(contract,null,2)+'\n');
console.log(JSON.stringify({result:contract.result,...contract.measurements},null,2));
