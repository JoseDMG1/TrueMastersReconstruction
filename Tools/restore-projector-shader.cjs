const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..'), apply=process.argv.includes('--apply');
const source=path.join(root,'Tools/shader-candidates/dynamic-shadow-projector');
const target=path.join(root,'UnityProject/Assets/Shader');
const shader='DynamicShadowProjector_Projector_Dynamic_Shadow With Linear Falloff.shader';
const pairs=[['DynamicShadowLinearFalloff.shader',shader],['ProjectorCandidate.cginc','ProjectorCandidate.cginc'],['LICENSE','ReconstructedProjector-LICENSE.txt']];
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
const expected='f3949c9f041451b43ad8bc0c547816eb81f8eca58704d6932ec1463b96ac1b16';
const current=fs.readFileSync(path.join(target,shader));
const next=fs.readFileSync(path.join(source,pairs[0][0]));
if(sha(current)!==expected&&!current.equals(next))throw Error('Unexpected shader body; review instead of overwriting');
const changed=pairs.filter(([a,b])=>!fs.existsSync(path.join(target,b))||!fs.readFileSync(path.join(source,a)).equals(fs.readFileSync(path.join(target,b))));
const backup=path.join(root,'Evidence/WorkingBackups/projector-shader-'+new Date().toISOString().replace(/[:.]/g,'-'));
const manifest={generatedUtc:new Date().toISOString(),applied:apply,classification:'Approximate reconstruction from MIT projection reference; not original Asset Store source',limitation:'Dynamic shadow texture generation remains unimplemented. No directional light, stage material or scene edits.',backup:apply&&changed.length?backup:null,changes:changed.map(([a,b])=>({source:path.relative(root,path.join(source,a)),target:path.relative(root,path.join(target,b)),beforeSha256:fs.existsSync(path.join(target,b))?sha(fs.readFileSync(path.join(target,b))):null,afterSha256:sha(fs.readFileSync(path.join(source,a)))})),shaderMetaSha256:sha(fs.readFileSync(path.join(target,shader+'.meta')))};
if(apply&&changed.length){fs.mkdirSync(backup,{recursive:true});for(const [a,b] of changed){if(fs.existsSync(path.join(target,b)))fs.copyFileSync(path.join(target,b),path.join(backup,b));fs.copyFileSync(path.join(source,a),path.join(target,b));}fs.writeFileSync(path.join(root,'Evidence/projector-shader-restoration.json'),JSON.stringify(manifest,null,2)+'\n');}
console.log(JSON.stringify(manifest,null,2));
