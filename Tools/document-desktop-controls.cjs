const fs=require('fs'),crypto=require('crypto');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const [desktopPath,menuPath,buildPath]=process.argv.slice(2);
if(!desktopPath||!menuPath||!buildPath) throw Error('Supply exact desktop probe, menu probe and archived build report');
const desktop=read(desktopPath),menu=read(menuPath),build=read(buildPath),launch=read(desktopPath.replace(/\.json$/,'-launch.json'));
for(const r of [desktop,menu]) if(r.result!=='passed'||r.errors.length) throw Error('Selected runtime report did not pass');
if(build.result!=='Succeeded'||build.errors) throw Error('Selected build did not succeed');
const observed=desktop.observations.filter(o=>o.step.startsWith('desktop-'));
if(observed.filter(o=>o.step==='desktop-accepted').length!==2||observed.filter(o=>o.step==='desktop-move').length!==8) throw Error('Both keyboard layouts not accepted');
const acceptedDetails=observed.filter(o=>o.step==='desktop-accepted').map(o=>o.detail).join(' | ');
if(!acceptedDetails.includes('Arrows/Z/X/C')||!acceptedDetails.includes('WASD/left click/Shift/Space')) throw Error('Selected reports do not exercise the current bindings');
const sources=['InputReader','GameController','JoystickSettings'].map(name=>{const path=`UnityProject/Assets/Scripts/Assembly-CSharp/${name}.cs`;return {path,sha256:sha(path)};});
const screenshots=[1,2].map(i=>{const path=desktopPath.replace(/\.json$/,`.controls-${i}.png`);return {path,sha256:sha(path),inspection:'WASD + RATÓN option readable within extended settings background; existing MUT/INV clipping and overexposed character remain separate visual gaps'};});
const result={generatedUtc:new Date().toISOString(),status:'observed-bounded-Windows-runtime',sources,
 layouts:{default:{movement:'arrow keys',attack:'Z',evade:'X',super:'C'},alternate:{movement:'W/A/S/D',attack:'left mouse',evade:'left Shift',super:'Space'},preference:'AlternateKeyboardControls',settingsLabel:'WASD + ratón'},
 desktop:{path:desktopPath,sha256:sha(desktopPath),gameplayAssemblySha256:launch.GameplayAssemblySha256,driver:'Windows keybd_event with mapped scan codes and mouse_event; focus verified before every press; no direct InputSet/animator injection for physical input assertions',observations:observed},
 menuRegression:{path:menuPath,sha256:sha(menuPath),observations:menu.observations.filter(o=>o.step.startsWith('control-')||o.step.startsWith('settings'))},
 build:{path:buildPath,sha256:sha(buildPath),report:build,currentGameplayAssemblySha256:sha('Builds/Windows/TrueMastersOffline_Data/Managed/Assembly-CSharp.dll')},screenshots,
 boundaries:['Sora versus disabled CPU in Windows reconstruction; not all roster/awakened directional variants or original APK behavior.','ChargeSpecial accelerates readiness; OS buttons triggered awakening and phase1 special; natural charging not accepted.','Toggle exercised through UGUI PointerClick callback and persisted across scene reload; no physical OS checkbox click claim.','Both runtime probes use the rebuilt current Z/X/C and left-click/Shift/Space gameplay assembly. Neutral shield has not been exercised; right Shift is not bound in the current source.','Prior failed desktop reports retained; scan-code/focus driver changes preceded passing run, exact prior phase1 input-delivery cause not isolated.','Original save bytes and captured preference presence/values, including alternate layout, restored.','Pause/focus reset implemented; full physical focus-loss/pause acceptance not performed.']};
fs.writeFileSync('Evidence/desktop-controls-acceptance.json',JSON.stringify(result,null,2)+'\n');
console.log(JSON.stringify({sources:sources.length,moves:observed.filter(o=>o.step==='desktop-move').length,layouts:observed.filter(o=>o.step==='desktop-accepted').map(o=>o.detail),menu:menu.result,build:build.result}));
