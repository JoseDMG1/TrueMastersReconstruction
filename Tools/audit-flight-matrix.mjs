import fs from 'node:fs';
import crypto from 'node:crypto';
import assert from 'node:assert/strict';
const [matrixPath,awakePath,buildPath,movingPath]=process.argv.slice(2);
assert(matrixPath&&awakePath&&buildPath,'Supply explicit FlightMatrix runtime, AwakeJumps runtime and archived build paths.');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const identity=path=>({path,sha256:hash(path)});
const inventory=read('Tools/unity-validation/FlightMatrixProfiles.json');
for(const p of [...inventory.profiles,...inventory.jumps])assert.equal(hash(p.file),p.sha256);
const runtime=read(matrixPath),awakeRuntime=read(awakePath),build=read(buildPath);
const matrixLaunch=read(matrixPath.replace(/\.json$/,'-launch.json')),awakeLaunch=read(awakePath.replace(/\.json$/,'-launch.json'));
for(const r of [runtime,awakeRuntime]){assert.equal(r.result,'passed');assert.deepEqual(r.errors,[]);assert(r.observations.some(o=>o.step==='fixture-restored'));}
assert.equal(matrixLaunch.Scenario,'FlightMatrix');assert.equal(awakeLaunch.Scenario,'AwakeJumps');
assert.equal(matrixLaunch.GameplayAssemblySha256,awakeLaunch.GameplayAssemblySha256);
assert.equal(hash(matrixLaunch.GameplayAssembly),matrixLaunch.GameplayAssemblySha256);
assert.equal(build.result,'Succeeded');assert.equal(build.errors,0);
const tracePath=matrixPath.replace(/\.json$/,'.matrix.json'),matrix=read(tracePath);
assert.equal(matrix.result,'passed');assert.equal(matrix.cases.length,inventory.profiles.length*8);
assert.equal(inventory.profiles.filter(p=>p.follow).length,33);assert.equal(inventory.profiles.filter(p=>!p.follow&&p.light).length,13);
for(const p of inventory.profiles)for(const side of [0,1])for(const scenario of ['moving','height','edge','interrupt']){
    const cases=matrix.cases.filter(c=>c.profile===p.name&&c.side===side&&c.scenario===scenario);assert.equal(cases.length,1);
    const c=cases[0];assert.deepEqual(c.errors,[]);assert(c.restored);assert(c.correctTarget);
    assert(c.rotationError<=.15&&c.translationError<=.002&&c.targetDrift<=.01);
    assert(c.frames>=4||(p.edge&&scenario==='edge'&&c.destroyed));
    if(scenario==='interrupt'){assert(c.ownerDamageAfter>c.ownerDamageBefore);if(c.registeredAtHit)assert(c.weakObserved);}
    if(scenario==='edge'&&p.edge){assert(c.destroyed);assert(c.maximumRadius>33);}
    if(p.light&&!p.follow&&scenario!=='edge'){assert(c.fixedMoves>=3);assert(c.fixedHeadingFrames>=3);}
    if(p.dormantFollow)assert(c.dormantFollowChecked);
    else if(p.follow)assert(c.targetFrames>=3||(p.edge&&scenario==='edge'&&c.destroyed));
    if(c.attributedHit)assert(c.targetDamageAfter>c.targetDamageBefore&&c.damageFrames.some(d=>c.contactFrames.some(f=>Math.abs(f-d)<=1)));
}
assert.equal(awakeRuntime.observations.filter(o=>o.step==='awake-jump').length,inventory.jumps.length);
const jumps=inventory.jumps.map((j,i)=>{
    const path=awakePath.replace(/\.json$/,`.jump-${i}.json`),r=read(path);
    assert.equal(r.result,'passed');assert.deepEqual(r.errors,[]);assert(r.entered&&r.recovered&&r.awakeningActive&&r.restored);
    assert.equal(r.fighter,j.fighter);assert.equal(r.state,j.state);assert.equal(r.expected,j.name);assert(r.spawnPositionError<=.001);
    assert(r.flight.frames>=4&&r.flight.restored);assert.deepEqual(r.flight.errors,[]);
    if(j.fighter==='Raven'){assert(r.flight.correctTarget);assert(r.flight.rotationError<=.15);assert(r.flight.turns>0);}
    else assert(r.flight.horizontalTravel<=.01);
    return {...identity(path),fighter:j.fighter,state:j.state,object:j.name,frames:r.flight.frames,turns:r.flight.turns,contacts:r.flight.contacts,targetDamage:r.targetDamage,image:identity(path.replace(/\.json$/,'.png'))};
});
const sources=['OfflinePlayerProbe.cs','OfflineFlightMatrixProbe.cs','OfflineAwakeJumpProbe.cs','OfflineMovingTargetProbe.cs'];
for(const name of sources)assert.equal(hash('Tools/unity-validation/'+name),hash('UnityProject/Assets/ReconstructionValidation/'+name));
assert.equal(hash('Tools/unity-validation/FlightMatrixProfiles.json'),hash('UnityProject/Assets/ReconstructionValidation/Resources/FlightMatrixProfiles.json'));
let movingTargetRegression=null;
if(movingPath){
    const r=read(movingPath),flightPath=movingPath.replace(/\.json$/,'.flight.json'),f=read(flightPath),launchPath=movingPath.replace(/\.json$/,'-launch.json'),launch=read(launchPath);
    assert.equal(r.result,'passed');assert.deepEqual(r.errors,[]);assert.equal(f.result,'passed');assert(f.restored);assert.equal(launch.GameplayAssemblySha256,matrixLaunch.GameplayAssemblySha256);
    assert.equal(launch.Scenario,'MovingTarget');assert(f.finalDamage>f.initialDamage&&f.damageEvents.some(d=>f.contacts.some(c=>Math.abs(c-d.frame)<=1)));
    movingTargetRegression={runtime:identity(movingPath),flight:identity(flightPath),launch:identity(launchPath),acceptance:identity('Evidence/moving-target-flight-acceptance.json')};
}
const result={generatedUtc:new Date().toISOString(),result:'passed-bounded-flight-matrix-and-awake-evades',
    runtime:identity(matrixPath),trace:identity(tracePath),awakeRuntime:identity(awakePath),build:identity(buildPath),
    launches:[identity(matrixPath.replace(/\.json$/,'-launch.json')),identity(awakePath.replace(/\.json$/,'-launch.json'))],assembly:identity(matrixLaunch.GameplayAssembly),
    profiles:inventory.profiles.length,followProfiles:33,activeRootFollowProfiles:inventory.profiles.filter(p=>p.follow&&!p.dormantFollow).length,dormantFollowProfiles:inventory.profiles.filter(p=>p.dormantFollow).map(p=>p.name),straightLightProfiles:13,cases:matrix.cases.length,jumps,movingTargetRegression,
    interruptedRegistered:matrix.cases.filter(c=>c.scenario==='interrupt'&&c.registeredAtHit).length,interruptedUnregistered:matrix.cases.filter(c=>c.scenario==='interrupt'&&!c.registeredAtHit).map(c=>({profile:c.profile,side:c.side,weakObserved:c.weakObserved})),
    attributedContactCases:matrix.cases.filter(c=>c.attributedHit).map(c=>({profile:c.profile,side:c.side,scenario:c.scenario,damage:c.targetDamageAfter-c.targetDamageBefore})),
    maximumRotationError:Math.max(...matrix.cases.map(c=>c.rotationError)),maximumTranslationError:Math.max(...matrix.cases.map(c=>c.translationError)),
    sources:[...sources.map(n=>'Tools/unity-validation/'+n),'Tools/unity-validation/FlightMatrixProfiles.json','Tools/build-flight-matrix.mjs','Tools/audit-flight-matrix.mjs','Tools/Build-OfflinePlayer.ps1','Tools/Run-OfflineProbe.ps1'].map(identity),
    authored:[...new Set([...inventory.profiles.map(p=>p.file),...inventory.jumps.map(j=>j.file)])].map(identity),
    native:['Follow','GoForward','MoveLight','MoveWater','MoveRock','MoveAttack','DamageOnCollision','DestroyOnEdge','SummonAttack','ChangeStartPosition','StartRotation','IncreaseSize'].map(n=>identity('Evidence/native-disassembly-'+n+'.txt')),
    limits:[matrix.limitation,'Five actual awake evade routes; charge accelerated, target held. Raven airborne contacts and Maya trap semantics are distinct. Not every natural normal attack moving-target impact, every geometry, original runtime/VFX parity, online or Android.','Native evidence uses retained original metadata/corrected local Capstone per REA skill, not a connected REA MCP server.']};
fs.writeFileSync('Evidence/flight-matrix-acceptance.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({result:result.result,profiles:result.profiles,cases:result.cases,jumps:jumps.length,maximumRotationError:result.maximumRotationError,maximumTranslationError:result.maximumTranslationError},null,2));
